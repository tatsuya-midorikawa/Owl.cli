namespace Owl.cli

open System.Runtime.InteropServices

[<System.Runtime.Versioning.SupportedOSPlatform("macOS")>]
[<System.Runtime.Versioning.SupportedOSPlatform("Linux")>]
module zsh =
  let private zsh' =
    if RuntimeInformation.IsOSPlatform OSPlatform.OSX
      then "/bin/zsh"
      else "/usr/bin/zsh"

  let private psi' = System.Diagnostics.ProcessStartInfo (zsh', 
    // enable commnads input and reading of output
    UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true,
    // hide console window
    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden, CreateNoWindow = true)
    
  [<System.Runtime.Versioning.SupportedOSPlatform("macOS")>]
  [<System.Runtime.Versioning.SupportedOSPlatform("Linux")>]
  type ZshBuilder () =
    inherit ShellBuilder(psi')
  
  [<System.Runtime.Versioning.SupportedOSPlatform("macOS")>]
  [<System.Runtime.Versioning.SupportedOSPlatform("Linux")>]
  let zsh () = new ZshBuilder()