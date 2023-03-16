namespace Owl.cli

module cli =
  type CliBuilder (psi: System.Diagnostics.ProcessStartInfo) =
    inherit ShellBuilder(psi)
    
  let cli (shell_path: string) = 
    let psi = System.Diagnostics.ProcessStartInfo (shell_path, 
      // enable commnads input and reading of output
      UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true,
      // hide console window
      WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden, CreateNoWindow = true)
    new CliBuilder(psi)