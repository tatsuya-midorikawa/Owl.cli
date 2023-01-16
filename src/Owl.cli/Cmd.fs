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
  
  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  type CmdBuilder () =
    let prc' = Process.Start psi'
    let stdout' = StringBuilder()
    let mutable cnt = 0
    do
      prc'.OutputDataReceived.Add (fun e ->
        if 3 < cnt && e.Data <> "" && e.Data <> null then 
          stdout'.AppendLine e.Data |> ignore
        cnt <- cnt + 1
      )
      prc'.BeginOutputReadLine()

    member __.Yield (_) = __
    //member __.Zero () = ()
      //prc.StandardInput.WriteLine "exit"
      //prc.WaitForExit()
      //stdout'.ToString ()

    [<CustomOperation("ls")>]
    member __.ls (_, path: string) =
      task { do! prc'.StandardInput.WriteLineAsync $"dir \"{path}\""} |> Task.WaitAll
    [<CustomOperation("dir")>]
    member __.dir (_, path: string) =
      task { do! prc'.StandardInput.WriteLineAsync $"dir \"{path}\""} |> Task.WaitAll
    [<CustomOperation("cd")>]
    member __.cd (_, path: string) =
      task { do! prc'.StandardInput.WriteLineAsync $"cd \"{path}\""} |> Task.WaitAll
    [<CustomOperation("systeminfo")>]
    member __.systeminfo (_) =
      task { do! prc'.StandardInput.WriteLineAsync $"systeminfo"} |> Task.WaitAll
      
    [<CustomOperation("exit")>]
    member __.Exit (_) =
      task {
        do! prc'.StandardInput.WriteLineAsync "exit"
        do! prc'.WaitForExitAsync()
        return stdout'.ToString ()
      }
      |> (fun t -> t.Result)

    interface IDisposable with
      member __.Dispose() = prc'.Dispose ()

  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  let cmd = new CmdBuilder()