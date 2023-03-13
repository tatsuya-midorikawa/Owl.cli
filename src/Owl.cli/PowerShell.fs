namespace owl.cli

module powershell =
  open System
  open System.Diagnostics
  open System.Text
  open System.Threading.Tasks

  let private pwsh' = "powershell"
  let private psi' = ProcessStartInfo (pwsh', 
    // enable commnads input and reading of output
    UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true,
    // hide console window
    WindowStyle = ProcessWindowStyle.Hidden, CreateNoWindow = true)
    
  type state = Stop | Running
  type command = Command of string

  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  type PowerShellBuilder () =
    let ignore' = [| "\f"; "\r"; "\n";  "\r\n"; "";  |]
    let prc' = Process.Start psi'
    let stdout' = StringBuilder()
    let mutable state' = Stop
    let mutable cnt = 0L
    do
      state' <- Running
      prc'.OutputDataReceived.Add (fun e ->
        if 3L < cnt && e.Data <> null
        //if 3L < cnt && ignore'|> Array.contains e.Data |> not && e.Data <> null
          then stdout'.AppendLine e.Data |> ignore
        cnt <- cnt + 1L
      )
      prc'.BeginOutputReadLine()

    member __.Yield (_) = __
    member __.Result () =
      if state' = Running
        then __.exit prc' |> ignore
      task { do! prc'.WaitForExitAsync () } |> Task.WaitAll
      stdout'.ToString()
      
    [<CustomOperation("exec")>]
    member __.exec (_, cmd: string) =
      if state' = Running
        then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __
    [<CustomOperation("exec")>]
    member __.exec (state, Command cmd) =
      __.exec (state, cmd)

    [<CustomOperation("exit")>]
    member __.exit (_: obj) =
      if state' = Running
        then
          task { do! prc'.StandardInput.WriteLineAsync "exit" } |> Task.WaitAll
          state' <- Stop
      __
      
    [<CustomOperation("GetWmiObject")>]
    member __.cd (state, cmd) =
      __.exec (state, $"Get-WmiObject %s{cmd}")


    interface IDisposable with
      member __.Dispose() = prc'.Dispose ()

    
  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  let powershell () = new PowerShellBuilder()