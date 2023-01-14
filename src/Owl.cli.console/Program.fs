open owl.cli

// let a = System.Diagnostics.Process.Start "/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal"
Zsh.exec [| "ls ./" |]
|> printfn "%s"
