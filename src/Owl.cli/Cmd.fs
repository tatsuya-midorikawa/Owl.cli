namespace owl.cli

[<AutoOpen>]
module Cmd =
  open System
  open System.Diagnostics
  open System.Text
  open System.Threading.Tasks

  let private cmd' = Environment.GetEnvironmentVariable "ComSpec"
  let private psi' = ProcessStartInfo (cmd', 
    // enable commnads input and reading of output
    UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true,
    // hide console window
    WindowStyle = ProcessWindowStyle.Hidden, CreateNoWindow = true)

  //let internal chain (cmds: seq<string>) = [| cmds |> String.concat " >> " |]
  //let internal chains (cmds: seq<string[]>) = cmds |> Seq.map (String.concat " >> ")

  //let run'as (wait'for'exit: bool) (cmd: string)  =
  //  let pi = ProcessStartInfo (cmd, 
  //    UseShellExecute = true,
  //    // hide console window
  //    CreateNoWindow = true,
  //    // run as adminstrator
  //    Verb = "runas")
    
  //  if wait'for'exit
  //  then
  //    use p = Process.Start pi
  //    p.WaitForExit()
  //  else
  //    Process.Start pi |> ignore

  let private exec (cmds: seq<string>) =
    let pi = ProcessStartInfo (cmd',
      // enable commnads input and reading of output
      UseShellExecute = false,
      RedirectStandardInput = true,
      RedirectStandardOutput = true,
      // hide console window
      CreateNoWindow = true)
    
    use p = Process.Start pi
    let stdout = StringBuilder()
    p.OutputDataReceived.Add (fun e -> if e.Data <> null then stdout.AppendLine(e.Data) |> ignore)
    p.BeginOutputReadLine()
    for cmd in cmds do 
      p.StandardInput.WriteLine cmd
    p.StandardInput.WriteLine "exit"
    p.WaitForExit()
    stdout.ToString()

  // let exec' (cmds: seq<string>) =
  //   let pi = ProcessStartInfo (cmd, 
  //     // enable commnads input and reading of output
  //     UseShellExecute = false,
  //     RedirectStandardInput = true,
  //     RedirectStandardOutput = true,
  //     // hide console window
  //     CreateNoWindow = true)

  //   let p = proc.start pi
  //   for cmd in cmds do p.exec cmd
  //   p
  
  type State = Stop | Running

  // https://learn.microsoft.com/ja-jp/windows-server/administration/windows-commands/windows-commands?source=recommendations
  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  type CmdBuilder () =
    let ignore' = [| "\f"; "\r"; "\n";  "\r\n"; "";  |]
    let prc' = Process.Start psi'
    let stdout' = StringBuilder()
    let mutable state' = Stop
    let mutable cnt = 0L
    do
      state' <- Running
      prc'.OutputDataReceived.Add (fun e ->
        if 3L < cnt && ignore'|> Array.contains e.Data |> not && e.Data <> null then 
          stdout'.AppendLine e.Data |> ignore
        cnt <- cnt + 1L
      )
      prc'.BeginOutputReadLine()

    member __.Yield (_) = __
    member __.Result () =
      if state' = Running then
        __.exit prc' |> ignore
      task { do! prc'.WaitForExitAsync () } |> Task.WaitAll
      stdout'.ToString()

    [<CustomOperation("exec")>]
    member __.exec (_, cmd: string) =
      task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll; __;
      
    // === A ===

    // === B ===

    // === C ===
    [<CustomOperation("cd")>]
    member __.cd (_, path: string) =
      task { do! prc'.StandardInput.WriteLineAsync $"cd \"{path}\"" } |> Task.WaitAll; __;
    [<CustomOperation("cls")>]
    member __.cls (_) =
      task { do! prc'.StandardInput.WriteLineAsync $"cls" } |> Task.WaitAll; __;

    // === D ===
    [<CustomOperation("dir")>]
    member __.dir (_, path: string) =
      task { do! prc'.StandardInput.WriteLineAsync $"dir \"{path}\"" } |> Task.WaitAll; __;

    // === E ===
    [<CustomOperation("exit")>]
    member __.exit (_: obj) =
      task { do! prc'.StandardInput.WriteLineAsync "exit" } |> Task.WaitAll
      state' <- Stop
      __

    // === F ===
    // === G ===
    // === H ===
    // === I ===
    // === J ===
    // === K ===
    // === L ===
    [<CustomOperation("ls")>]
    member __.ls (_, path: string) =
      task { do! prc'.StandardInput.WriteLineAsync $"dir \"{path}\"" } |> Task.WaitAll; __

    // === M ===
    // === N ===
    // === O ===
    // === P ===
    // === Q ===
    // === R ===
    // === S ===
    [<CustomOperation("systeminfo")>]
    member __.systeminfo (_: obj) =
      task { do! prc'.StandardInput.WriteLineAsync $"systeminfo" } |> Task.WaitAll; __

    // === T ===
    // === U ===
    // === V ===
    // === W ===
    // === X ===
    // === Y ===
    // === Z ===
      

    interface IDisposable with
      member __.Dispose() = prc'.Dispose ()

  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  let cmd = new CmdBuilder()