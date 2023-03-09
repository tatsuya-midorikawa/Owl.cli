open owl.cli.cmd
open owl.cli.powershell
open System

//let c = cmd {
//  exec ((cd "C:\\") <&&> dir)
//  exec systeminfo
//  exec cls
//  exit
//}

//let c = cmd () {
//  //dir "." (<&&>) cd' [@"C:\logs"]
//  //dir "." (.>>) @"C:\logs\dir.txt"
//  cd @"C:\logs"
//  //cd @"C:\logs" (<&>) dir'
//  cd @"C:\logs" (<&&>) dir' [@".\"]
//  systeminfo (.>>) @".\sysinfo.log"
//  //reg add @"HKLM\SOFTWARE\Policies\Microsoft\Midoliy" ["/v Sample"; "/t REG_SZ"; "/d foo"]
//  reg query @"HKLM\SOFTWARE\Policies\Microsoft"
//  copy "foo" "bar"
//  copy ["/v"; "/d";] "src" "dst"
//  netsh trace start ["scenario=InternetClient_dbg";"tracefile=C:\\logs\\protocols.etl"; "capture=yes";"maxSize=500"]
//  netsh trace stop
//  psr start ["/output C:\\logs\\psr.zip";"/maxsc 999";"/gui 0"]
//  psr stop
//  //dsregcmd "/status"
//  dsregcmd "/status" (.>>) @"C:\logs\dsregcmd.log"
//  gpresult ["/h C:\logs\gpresult.html";"/f"]
//  gpresult ["/z"] (.>>) @"C:\logs\gpresult.log"
//  whoami (.>>) @"C:\logs\whoami.log"
//  cmdkey ["/list"] (.>>) @"C:\logs\whoami.log"
//  //schtasks change ["/tn Virus Check"; @"/tr C:\VirusCheck2.exe"]
//  //schtasks create ["/sc hourly"; "/mo 5"; "/sd 03/01/2002"; "/tn My App"; @"/tr c:\apps\myapp.exe"]
//  //schtasks delete ["/tn Start Mail"; "/s Svr16"]
//  //schtasks end' ["/tn \"My Notepad\""]
//  schtasks query (.>>) @"C:\logs\whoami.log"
//  schtasks query ["/V"; "/FO LIST";] (.>>) @"C:\logs\whoami.log"
//  //schtasks run ["/tn Security Script"]
//  wmic ["qfe"; "list";] (.>>) @"C:\logs\qfe.log"
//  exit
//}
//c.Result() |> printfn "%s"


let p = powershell () {
  GetWmiObject "win32_service"
}
p.Result() |> printfn "%s"

printfn ""