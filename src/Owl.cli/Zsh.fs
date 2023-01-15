namespace owl.cli

[<System.Runtime.Versioning.SupportedOSPlatform("MacCatalyst")>]
module Zsh =
  open System.Diagnostics
  open System.Text

  [<Literal>]
  let private zsh' = "/bin/zsh"
  // let private zsh' = "/System/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal"

  let exec (cmds: seq<string>) =
    use p = 
      ProcessStartInfo (zsh',
        UseShellExecute = false,
        RedirectStandardInput = true,
        RedirectStandardOutput = true,
        CreateNoWindow = true)
      |> System.Diagnostics.Process.Start

    let stdout = StringBuilder()
      
    p.OutputDataReceived.Add (fun e -> if e.Data <> null then stdout.AppendLine(e.Data) |> ignore)
    p.BeginOutputReadLine()
    cmds |> Seq.iter p.StandardInput.WriteLine
    p.StandardInput.WriteLine "exit"
    p.WaitForExit()
    stdout.ToString()