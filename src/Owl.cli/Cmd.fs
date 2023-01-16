namespace owl.cli

[<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
module Cmd =
  open System
  open System.Diagnostics
  open System.Text

  let private cmd = Environment.GetEnvironmentVariable "ComSpec"

  let internal chain (cmds: seq<string>) = [| cmds |> String.concat " >> " |]
  let internal chains (cmds: seq<string[]>) = cmds |> Seq.map (String.concat " >> ")

  let run'as (wait'for'exit: bool) (cmd: string)  =
    let pi = ProcessStartInfo (cmd, 
      UseShellExecute = true,
      // hide console window
      CreateNoWindow = true,
      // run as adminstrator
      Verb = "runas")
    
    if wait'for'exit
    then
      use p = Process.Start pi
      p.WaitForExit()
    else
      Process.Start pi |> ignore

  let exec (cmds: seq<string>) =
    let pi = ProcessStartInfo (cmd, 
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