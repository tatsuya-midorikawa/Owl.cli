namespace Owl.cli

open System.Runtime.InteropServices

[<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
[<System.Runtime.Versioning.SupportedOSPlatform("macOS")>]
module pwsh =
  let inline private get'install'path () =
    use key = Microsoft.Win32.Registry.LocalMachine
    use sub = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\pwsh.exe")
    if sub = Unchecked.defaultof<_> || sub.GetValue("") = Unchecked.defaultof<_>
      then raise (exn "'pwsh' is not installed.")
      else sub.GetValue("") |> string

  let private pwsh' =
    if RuntimeInformation.IsOSPlatform OSPlatform.Windows
      then get'install'path ()
    elif RuntimeInformation.IsOSPlatform OSPlatform.OSX
      then "/usr/local/bin/pwsh"
    else
      raise (System.NotSupportedException "Only supports Windows and macOS.")

  let private psi' = System.Diagnostics.ProcessStartInfo (pwsh', 
    // enable commnads input and reading of output
    UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true,
    // hide console window
    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden, CreateNoWindow = true)
    
  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  [<System.Runtime.Versioning.SupportedOSPlatform("macOS")>]
  type PwshBuilder () =
    inherit MsshellBuilder(psi')
  
  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  [<System.Runtime.Versioning.SupportedOSPlatform("macOS")>]
  let pwsh () = new PwshBuilder()