open owl.cli
open System


// let a = System.Diagnostics.Process.Start "/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal"
// Zsh.exec [| "ls ./" |]
// |> printfn "%s"

let c = cmd {
  cd "C:/"
  ls "./"
  systeminfo
  exec "cls"
  exit
}

let s = c.Result()

printfn "%A" s