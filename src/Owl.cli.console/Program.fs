﻿open owl.cli.cmd
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
  cd @"C:\logs"
  //cd @"C:\logs" (<&>) dir'
  cd @"C:\logs" (<&&>) dir' [@".\"]
  systeminfo (.>>) @".\sysinfo.log"
  //reg add @"HKLM\SOFTWARE\Policies\Microsoft\Midoliy" ["/v Sample"; "/t REG_SZ"; "/d foo"]
  reg query @"HKLM\SOFTWARE\Policies\Microsoft"
  copy "foo" "bar"
  copy ["/v"; "/d";] "src" "dst"
  netsh trace start ["scenario=InternetClient_dbg";"tracefile=C:\\logs\\protocols.etl"; "capture=yes";"maxSize=500"]
  netsh trace stop
  psr start ["/output C:\\logs\\psr.zip";"/maxsc 999";"/gui 0"]
  psr stop
  exit
}
c.Result() |> printfn "%s"

printfn ""