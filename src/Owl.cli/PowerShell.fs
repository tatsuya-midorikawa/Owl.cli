namespace Owl.cli

module powershell =
  let private pwsh' = "powershell"
  let private psi' = System.Diagnostics.ProcessStartInfo (pwsh', 
    // enable commnads input and reading of output
    UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true,
    // hide console window
    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden, CreateNoWindow = true)
  
  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  type PowerShellBuilder () =
    inherit ShellBuilder(psi')
    
  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  let powershell () = new PowerShellBuilder()
    
  //[<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  //type PowerShellBuilder () =
  //  let output' = ResizeArray<Output>()
  //  let prc' = Process.Start psi'
  //  let mutable state' = Stop
  //  do
  //    state' <- Running
  //    prc'.StandardInput.WriteLine("clear")
  //    let mutable s = prc'.StandardOutput.ReadLine()
  //    while not <| s.EndsWith "clear" do
  //      s <- prc'.StandardOutput.ReadLine()

  //  member __.Yield (x) = x
  //  member __.For (x, f) = f x
  //  member __.Zero () = __
      
  //  [<CustomOperation("exec", AllowIntoPattern=true)>]
  //  member __.exec (v, [<ProjectionParameter>] cmd: unit -> string) =
  //    let cmd = cmd()
  //    let acc = StringBuilder()
  //    if state' = Running
  //      then           
  //        prc'.StandardInput.WriteLine (cmd)
  //        prc'.StandardInput.WriteLine (eoc')
  //        prc'.StandardOutput.ReadLine() |> ignore // Discard command string (cmd).
  //        let mutable s = prc'.StandardOutput.ReadLine()
  //        while not <| s.EndsWith eoc' do
  //          acc.Append $"{s}{Environment.NewLine}" |> ignore
  //          s <- prc'.StandardOutput.ReadLine()
  //        prc'.StandardOutput.ReadLine() |> ignore // Discard command string (echo).
  //        output'.Add { cmd = cmd; result = acc.ToString() }

  //    acc.ToString()

  //  [<CustomOperation("exit")>]
  //  member __.exit (state: obj) =
  //    let ret = __.exec (state, fun () -> "exit")
  //    state' <- Stop; ret
      
  //  [<CustomOperation("GetWmiObject")>]
  //  member __.cd (state, cmd) =
  //    __.exec (state, fun () -> $"Get-WmiObject %s{cmd}")

  //  interface IDisposable with
  //    member __.Dispose() = prc'.Dispose ()