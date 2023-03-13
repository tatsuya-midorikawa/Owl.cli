namespace owl.cli

module powershell =
  open System
  open System.Diagnostics
  open System.Text
  open System.Threading.Tasks

  [<Literal>]
  let private pwsh' = "powershell"
  [<Literal>]
  let private eoc' = "echo \"Owl.cli.console: End of command\""
  let private psi' = ProcessStartInfo (pwsh', 
    // enable commnads input and reading of output
    UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true,
    // hide console window
    WindowStyle = ProcessWindowStyle.Hidden, CreateNoWindow = true)
    
  type state = Stop | Running
  type command = Command of string

  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  type PowerShellBuilder () =
    let prc' = Process.Start psi'
    let mutable state' = Stop
    do
      state' <- Running
      prc'.StandardInput.WriteLine("clear")
      let mutable s = prc'.StandardOutput.ReadLine()
      while not <| s.EndsWith "clear" do
        s <- prc'.StandardOutput.ReadLine()

    member __.Yield (x) = x
    member __.For (x, f) = f x
    member __.Zero () = __
      
    [<CustomOperation("exec", AllowIntoPattern=true)>]
    member __.exec (v, [<ProjectionParameter>] cmd: unit -> string) =
      let acc = StringBuilder()
      if state' = Running
        then           
          prc'.StandardInput.WriteLine (cmd())
          prc'.StandardInput.WriteLine (eoc')
          prc'.StandardOutput.ReadLine() |> ignore // Discard command string (cmd).
          let mutable s = prc'.StandardOutput.ReadLine()
          while not <| s.EndsWith eoc' do
            acc.Append $"{s}{Environment.NewLine}" |> ignore
            s <- prc'.StandardOutput.ReadLine()
          prc'.StandardOutput.ReadLine() |> ignore // Discard command string (echo).
      acc.ToString()

    [<CustomOperation("exit")>]
    member __.exit (_: obj) =
      if state' = Running
        then
          task { do! prc'.StandardInput.WriteLineAsync "exit" } |> Task.WaitAll
          state' <- Stop
      __
      
    [<CustomOperation("GetWmiObject")>]
    member __.cd (state, cmd) =
      __.exec (state, fun () -> $"Get-WmiObject %s{cmd}")

    interface IDisposable with
      member __.Dispose() = prc'.Dispose ()

    
  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  let powershell () = new PowerShellBuilder()