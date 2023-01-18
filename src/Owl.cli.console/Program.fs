open owl.cli.cmd
open System

//let c = cmd {
//  exec ((cd "C:\\") <&&> dir)
//  exec systeminfo
//  exec cls
//  exit
//}

let c = cmd () {
  dir "." (<&&>) cd' [@"C:\logs"]
  dir "." (.>>) @"C:\logs\dir.txt"
  //cd @"C:\logs"
  //cd @"C:\logs" (<&>) dir'
  //cd @"C:\logs" (<&&>) dir' [@".\"]
  //systeminfo (.>>) @".\sysinfo.log"
  exit
}
c.Result() |> printfn "%s"

printfn ""