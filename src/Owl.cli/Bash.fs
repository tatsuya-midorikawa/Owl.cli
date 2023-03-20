namespace Owl.cli

open System.Runtime.InteropServices

[<System.Runtime.Versioning.SupportedOSPlatform("macOS")>]
// [<System.Runtime.Versioning.SupportedOSPlatform("Linux")>]
module bash =
  let private bash' =
    if RuntimeInformation.IsOSPlatform OSPlatform.OSX
      then "/bin/bash"
    // elif RuntimeInformation.IsOSPlatform OSPlatform.Linux
    //   then "/usr/bin/bash"
    // else
    //   raise (System.NotSupportedException "Only supports Linux and macOS.")
    else
      raise (System.NotSupportedException "Only supports macOS.")

  let private psi' = System.Diagnostics.ProcessStartInfo (bash', 
    // enable commnads input and reading of output
    UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true,
    // hide console window
    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden, CreateNoWindow = true)
    
  [<System.Runtime.Versioning.SupportedOSPlatform("macOS")>]
  // [<System.Runtime.Versioning.SupportedOSPlatform("Linux")>]
  type BashBuilder () =
    inherit ShellBuilder(psi')
  
  [<System.Runtime.Versioning.SupportedOSPlatform("macOS")>]
  // [<System.Runtime.Versioning.SupportedOSPlatform("Linux")>]
  let bash () = new BashBuilder()