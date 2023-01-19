open owl.cli.cmd
open System

//let c = cmd {
//  exec ((cd "C:\\") <&&> dir)
//  exec systeminfo
//  exec cls
//  exit
//}

let c = cmd () {
  //dir "." (<&&>) cd' [@"C:\logs"]
  //dir "." (.>>) @"C:\logs\dir.txt"
  //cd @"C:\logs"
  //cd @"C:\logs" (<&>) dir'
  //cd @"C:\logs" (<&&>) dir' [@".\"]
  //systeminfo (.>>) @".\sysinfo.log"
  //reg add [@"HKLM\SOFTWARE\Policies\Microsoft\Midoliy"]
  reg add [@"HKLM\SOFTWARE\Policies\Microsoft\Midoliy"; "/v Sample"; "/t REG_SZ"; "/d foo" ]
  reg query [@"HKLM\SOFTWARE\Policies\Microsoft\Midoliy"]
  exit
}
c.Result() |> printfn "%s"

printfn ""