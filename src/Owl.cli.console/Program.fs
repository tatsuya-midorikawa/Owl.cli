open owl.cli
open System


// let a = System.Diagnostics.Process.Start "/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal"
// Zsh.exec [| "ls ./" |]
// |> printfn "%s"

let s = cmd {
  cd "C:/"
  ls "./"
  systeminfo
  exit
}

printfn "%A" s