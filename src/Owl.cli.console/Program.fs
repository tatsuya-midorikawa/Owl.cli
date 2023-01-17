open owl.cli.cmd
open System


// let a = System.Diagnostics.Process.Start "/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal"
// Zsh.exec [| "ls ./" |]
// |> printfn "%s"

let c = cmd {
  exec ((cd "C:\\") <&&> dir)
  exec systeminfo
  //exec cls
  exit
}

let s = c.Result()

printfn "%A" s
printfn ""