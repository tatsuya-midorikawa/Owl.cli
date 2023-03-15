namespace Owl.cli
namespace Owl.cli

module cmd =
  let private cmd' = System.Environment.GetEnvironmentVariable "ComSpec"
  let private psi' = System.Diagnostics.ProcessStartInfo (cmd', 
    // enable commnads input and reading of output
    UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true,
    // hide console window
    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden, CreateNoWindow = true)

  [<RequireQualifiedAccess;NoEquality;NoComparison>]
  type Cmdline =
    | mkdir of string
    | cd of string option

  let mkdir path = Cmdline.mkdir path
  let cd path = Cmdline.mkdir path

  // https://learn.microsoft.com/ja-jp/windows-server/administration/windows-commands/windows-commands?source=recommendations
  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  type CmdBuilder () =
    inherit ShellBuilder(psi', "cls")

  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  let cmd () = new CmdBuilder()



////[<AutoOpen>]
//module cmd =
//  open System
//  open System.Diagnostics
//  open System.Text
//  open System.Threading.Tasks

//  let private cmd' = Environment.GetEnvironmentVariable "ComSpec"
//  [<Literal>]
//  let private eoc' = "echo \"Owl.cli.console: End of command\""
//  let private psi' = ProcessStartInfo (cmd', 
//    // enable commnads input and reading of output
//    UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true,
//    // hide console window
//    WindowStyle = ProcessWindowStyle.Hidden, CreateNoWindow = true)

//  type state = Stop | Running
//  type command = Command of string

//  let combine cmd args =
//    let args = match args with Some args -> args |> String.concat " " | None -> ""
//    cmd |> Option.map (fun (Command cmd) -> Command $"%s{cmd} %s{args}")

//  let escape (s: string) = 
//    if not(s.StartsWith('\"')) then $"\"%s{s}" else s
//    |> (fun s -> if not(s.EndsWith('\"')) then $"%s{s}\"" else s)
    
//  let build_opt args =
//    match args with Some args -> args |> Seq.reduce (fun a b -> $"%s{a} %s{b}") | _ -> ""
  
//  let build_arg args =
//    args |> Seq.reduce (fun a b -> $"%s{a} %s{b}")


//  // https://stackoverflow.com/questions/28889954/what-does-do-in-this-batch-file
//  (* ///------> *)
//  type op_str = string -> string option -> string
//  type op_cmd = string -> command option -> string
//  let build_op_str (cmd: string) (dst: string option) (op: op_str option) =
//    match op with Some op -> op cmd dst | None -> cmd
//  let build_op_cmd (cmd1: string) (cmd2: command option) (op: op_cmd option) =
//    match op with Some op -> op cmd1 cmd2 | None -> cmd1

//  let (.>) : op_str = fun lhs -> function Some rhs -> $"%s{lhs} > %s{rhs}" | None -> lhs
//  let (.>>) : op_str = fun lhs -> function Some rhs -> $"%s{lhs} >> %s{rhs}" | None -> lhs
//  let (<&>) : op_cmd = fun lhs -> function Some (Command rhs) -> $"%s{lhs} & %s{rhs}" | None -> lhs
//  let (<&&>) : op_cmd = fun lhs -> function Some (Command rhs) -> $"%s{lhs} && %s{rhs}" | None -> lhs
//  (* <------/// *)

//  type AddCmd = AddCmd of string
//  type ChangeCmd = ChangeCmd of string
//  type CompareCmd = CompareCmd of string
//  type CopyCmd = CopyCmd of string
//  type CreateCmd = CreateCmd of string
//  type DeleteCmd = DeleteCmd of string
//  type EndCmd = EndCmd of string
//  type ExportCmd = ExportCmd of string
//  type ImportCmd = ImportCmd of string
//  type LoadCmd = LoadCmd of string
//  type QueryCmd = QueryCmd of string
//  type RestoreCmd = RestoreCmd of string
//  type RunCmd = RunCmd of string
//  type SaveCmd = SaveCmd of string
//  type StartCmd = StartCmd of string
//  type StopCmd = StopCmd of string
//  type UnloadCmd = UnloadCmd of string
    
//  let add = AddCmd "add"
//  let change = ChangeCmd "change"
//  let compare = CompareCmd "compare"
//  let copy = CopyCmd "copy"
//  let create = CreateCmd "create"
//  let delete = DeleteCmd "delete"
//  let end' = EndCmd "end"
//  let export = ExportCmd "export"
//  let import = ImportCmd "import"
//  let load = LoadCmd "load"
//  let query = QueryCmd "query"
//  let restore = RestoreCmd "restore"
//  let run = RunCmd "run"
//  let save = SaveCmd "save"
//  let start = StartCmd "start"
//  let stop = StartCmd "stop"
//  let unload = UnloadCmd "unload"
  
//  type TraceCtx = TraceCtx of string  
//  let trace = TraceCtx "trace"

//  // https://learn.microsoft.com/ja-jp/windows-server/administration/windows-commands/windows-commands?source=recommendations
//  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
//  type CmdBuilder () =
//    let ignore' = [| "\f"; "\r"; "\n";  "\r\n"; "";  |]
//    let prc' = Process.Start psi'
//    let stdout' = StringBuilder()
//    let mutable state' = Stop
//    let mutable cnt = 0L
//    do
//      state' <- Running
//      prc'.StandardInput.WriteLine("clear")
//      let mutable s = prc'.StandardOutput.ReadLine()
//      while not <| s.EndsWith "clear" do
//        s <- prc'.StandardOutput.ReadLine()
//      //prc'.OutputDataReceived.Add (fun e ->
//      //  if 3L < cnt && ignore'|> Array.contains e.Data |> not && e.Data <> null
//      //    then stdout'.AppendLine e.Data |> ignore
//      //  cnt <- cnt + 1L
//      //)
//      //prc'.BeginOutputReadLine()
      
//    member __.Yield (x) = x
//    member __.For (x, f) = f x
//    member __.Zero () = __
//    //member __.Result () =
//    //  if state' = Running
//    //    then __.exit prc' |> ignore
//    //  task { do! prc'.WaitForExitAsync () } |> Task.WaitAll
//    //  stdout'.ToString()
      
      
//    [<CustomOperation("exec", AllowIntoPattern=true)>]
//    member __.exec (v, [<ProjectionParameter>] cmd: unit -> string) =
//      let acc = StringBuilder()
//      if state' = Running
//        then           
//          prc'.StandardInput.WriteLine (cmd())
//          prc'.StandardInput.WriteLine (eoc')
//          prc'.StandardOutput.ReadLine() |> ignore // Discard command string (cmd).
//          let mutable s = prc'.StandardOutput.ReadLine()
//          while not <| s.EndsWith eoc' do
//            acc.Append $"{s}{Environment.NewLine}" |> ignore
//            s <- prc'.StandardOutput.ReadLine()
//          prc'.StandardOutput.ReadLine() |> ignore // Discard command string (echo).
//      acc.ToString()

//    //[<CustomOperation("exec")>]
//    member private __.exec (v, cmd: string) =
//      __.exec(v, fun () -> cmd)
//      //if state' = Running
//      //  then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
//      //__

//    //[<CustomOperation("exec")>]
//    //member __.exec (state, Command cmd) =
//    //  __.exec (state, cmd)

//    [<CustomOperation("exit")>]
//    member __.exit (_: obj) =
//      if state' = Running
//        then
//          task { do! prc'.StandardInput.WriteLineAsync "exit" } |> Task.WaitAll
//          state' <- Stop
//      __
      
//    // === A ===

//    // === B ===
//    // === C ===
//    [<CustomOperation("cd")>]
//    member __.cd (state, path, ?op: op_cmd, ?cmd2: command, ?args: string list) =
//      let cmd = op |> build_op_cmd $"cd %s{path}" (combine cmd2 args)
//      __.exec (state, cmd)

//    [<CustomOperation("cls")>]
//    member __.cls (state, ?op: op_cmd, ?cmd2: command, ?args: string list) =
//      let cmd = op |> build_op_cmd $"cls" (combine cmd2 args)
//      __.exec (state, cmd)

//    [<CustomOperation("copy")>]
//    member __.copy (state, src, dst) =
//      __.exec (state, $"copy %s{src} %s{dst}")
//    [<CustomOperation("copy")>]
//    member __.copy (state, args, src, dst) =
//      __.exec (state, $"copy %s{build_arg args} %s{src} %s{dst}")
      
//    [<CustomOperation("cmdkey")>]      
//    member __.cmdkey (state, args: seq<string>, ?op: op_str, ?dst: string) =
//      let cmd = op |> build_op_str $"cmdkey %s{build_opt (Some args)}" dst
//      __.exec (state, cmd)

//    // === D ===
//    [<CustomOperation("dir")>]
//    member __.dir (state, ?path: string, ?op: op_str, ?dst: string) =
//      let dir = match path with Some p -> $"dir {p}" | None -> "dir"
//      let cmd = op |> build_op_str dir dst
//      __.exec (state, cmd)
//    [<CustomOperation("dir")>]
//    member __.dir (state, ?path: string, ?op: op_cmd, ?cmd2: command, ?args: string list) =
//      let dir = match path with Some p -> $"dir {p}" | None -> "dir"
//      let cmd = op |> build_op_cmd dir (combine cmd2 args)
//      __.exec (state, cmd)
    
//    (* DSREGCMD switches
//                        /? : Displays the help message for DSREGCMD
//                   /status : Displays the device join status
//               /status_old : Displays the device join status in old format
//                     /join : Schedules and monitors the Autojoin task to Hybrid Join the device
//                    /leave : Performs Hybrid Unjoin
//                    /debug : Displays debug messages
//               /refreshprt : Refreshes PRT in the CloudAP cache
//          /refreshp2pcerts : Refreshes P2P certificates
//          /cleanupaccounts : Deletes all WAM accounts
//             /listaccounts : Lists all WAM accounts
//             /UpdateDevice : Update device attributes to Azure AD *)
//    [<CustomOperation("dsregcmd")>]      
//    member __.dsregcmd (state, switch, ?op: op_str, ?dst: string) =
//      let cmd = op |> build_op_str $"dsregcmd %s{switch}" dst
//      __.exec (state, cmd)

//    // === E ===
//    // === F ===
//    // === G ===
//    [<CustomOperation("gpresult")>]
//    member __.gpresult (state, args: seq<string>, ?op: op_str, ?dst: string ) =
//      let cmd = op |> build_op_str $"gpresult %s{build_opt (Some args)}" dst
//      __.exec (state, cmd)

//    // === H ===
//    // === I ===
//    // === J ===
//    // === K ===
//    // === L ===
//    // === M ===
//    // === N ===
//    // TODO
//    // https://learn.microsoft.com/ja-jp/windows-server/networking/technologies/netsh/netsh-contexts
//    // https://learn.microsoft.com/ja-jp/previous-versions/windows/it-pro/windows-server-2008-R2-and-2008/cc754516(v=ws.10)
//    member __.netsh (state, ctx, cmd, args) =
//      __.exec (state, $"netsh %s{ctx} %s{cmd} %s{build_opt args}")
//    [<CustomOperation("netsh")>]
//    member __.netsh (state, ctx: TraceCtx, StartCmd cmd, ?args) =
//      let ctx = match ctx with TraceCtx c -> c
//      __.netsh(state, ctx, cmd, args)
//    [<CustomOperation("netsh")>]
//    member __.netsh (state, ctx: TraceCtx, StopCmd cmd) = 
//      let ctx = match ctx with TraceCtx c -> c
//      __.netsh(state, ctx, cmd, None)

//    // === O ===
//    // === P ===
//    [<CustomOperation("psr")>]
//    member __.psr (state, StartCmd cmd, ?args) =
//      __.exec (state, $"psr /%s{cmd} %s{build_opt args}")
//    [<CustomOperation("psr")>]
//    member __.psr (state, StopCmd cmd) =
//      __.exec (state, $"psr /%s{cmd}")

//    // === Q ===
//    // === R ===
//    [<CustomOperation("reg")>]
//    member __.reg (state, AddCmd addcmd, key, ?args) =
//      __.exec (state, $"reg %s{addcmd} \"%s{key}\" %s{build_opt args}")
//    [<CustomOperation("reg")>]
//    member __.reg (state, CompareCmd comparecmd, key1, key2, ?args) =
//      __.exec (state, $"reg %s{comparecmd} \"%s{key1}\" \"%s{key2}\" %s{build_opt args}")
//    [<CustomOperation("reg")>]
//    member __.reg (state, CopyCmd copycmd, key1, key2, ?args) =
//      __.exec (state, $"reg %s{copycmd} \"%s{key1}\" \"%s{key2}\" %s{build_opt args}")
//    [<CustomOperation("reg")>]
//    member __.reg (state, DeleteCmd deletecmd, key, ?args) =
//      __.exec (state, $"reg %s{deletecmd} \"%s{key}\" %s{build_opt args}")
//    [<CustomOperation("reg")>]
//    member __.reg (state, ExportCmd exportcmd, key, filename, ?args) =
//      __.exec (state, $"reg %s{exportcmd} \"%s{key}\" \"%s{filename}\" %s{build_opt args}")
//    [<CustomOperation("reg")>]
//    member __.reg (state, ImportCmd importcmd, filename, ?args) =
//      __.exec (state, $"reg %s{importcmd} \"%s{filename}\" %s{build_opt args}")
//    [<CustomOperation("reg")>]
//    member __.reg (state, LoadCmd loadcmd, key, filename) =
//      __.exec (state, $"reg %s{loadcmd} \"%s{key}\" \"%s{filename}\"")
//    [<CustomOperation("reg")>]
//    member __.reg (state, RestoreCmd restorecmd, key, filename) =
//      __.exec (state, $"reg %s{restorecmd} \"%s{key}\" \"%s{filename}\"")
//    [<CustomOperation("reg")>]
//    member __.reg (state, QueryCmd querycmd, key, ?args) =
//      __.exec (state, $"reg %s{querycmd} \"%s{key}\" %s{build_opt args}")
//    [<CustomOperation("reg")>]
//    member __.reg (state, SaveCmd savecmd, key, filename, ?args) =
//      __.exec (state, $"reg %s{savecmd} \"%s{key}\" \"%s{filename}\" %s{build_opt args}")
//    [<CustomOperation("reg")>]
//    member __.reg (state, UnloadCmd unloadcmd, key) =
//      __.exec (state, $"reg %s{unloadcmd} \"%s{key}\"")

//    // === S ===
//    [<CustomOperation("schtasks")>]
//    member __.schtasks (state, ChangeCmd change, ?args) =
//      __.exec (state, $"schtasks /%s{change} %s{build_opt args}")
//    [<CustomOperation("schtasks")>]
//    member __.schtasks (state, CreateCmd create, ?args) =
//      __.exec (state, $"schtasks /%s{create} %s{build_opt args}")
//    [<CustomOperation("schtasks")>]
//    member __.schtasks (state, DeleteCmd delete, ?args) =
//      __.exec (state, $"schtasks /%s{delete} %s{build_opt args}")
//    [<CustomOperation("schtasks")>]
//    member __.schtasks (state, EndCmd end', ?args) =
//      __.exec (state, $"schtasks /%s{end'} %s{build_opt args}")
//    [<CustomOperation("schtasks")>]
//    member __.schtasks (state, QueryCmd query, ?op: op_str, ?dst: string) =
//      let cmd = op |> build_op_str $"schtasks /%s{query}" dst
//      __.exec (state, cmd)
//    [<CustomOperation("schtasks")>]
//    member __.schtasks (state, QueryCmd query, ?args, ?op: op_str, ?dst: string) =
//      let cmd = op |> build_op_str $"schtasks /%s{query} %s{build_opt args}" dst
//      __.exec (state, cmd)
//    [<CustomOperation("schtasks")>]
//    member __.schtasks (state, RunCmd run, ?args) =
//      __.exec (state, $"schtasks /%s{run} %s{build_opt args}")
//    [<CustomOperation("systeminfo")>]
//    member __.systeminfo (state, ?op: op_str, ?dst: string) =
//      let cmd = op |> build_op_str $"systeminfo" dst
//      __.exec (state, cmd)

//    // === T ===
//    // === U ===
//    // === V ===
//    // === W ===
//    // === G ===
//    [<CustomOperation("whoami")>]
//    member __.whoami (state, ?op: op_str, ?dst: string ) =
//      let cmd = op |> build_op_str $"whoami" dst
//      __.exec (state, cmd)
//    [<CustomOperation("whoami")>]
//    member __.whoami (state, ?args, ?op: op_str, ?dst: string ) =
//      let cmd = op |> build_op_str $"whoami %s{build_opt args}" dst
//      __.exec (state, cmd)
//    [<Obsolete("The WMI command-line (WMIC) utility is deprecated as of Windows 10, version 21H1, and as of the 21H1 semi-annual channel release of Windows Server. This utility is superseded by Windows PowerShell for WMI (see Chapter 7—Working with WMI). This deprecation applies only to the WMI command-line (WMIC) utility; Windows Management Instrumentation (WMI) itself is not affected. Also see Windows 10 features we're no longer developing.")>]
//    [<CustomOperation("wmic")>]
//    member __.wmic (state, ?args, ?op: op_str, ?dst: string ) =
//      let cmd = op |> build_op_str $"wmic %s{build_opt args}" dst
//      __.exec (state, cmd)

//    // === X ===
//    // === Y ===
//    // === Z ===

//    interface IDisposable with
//      member __.Dispose() = prc'.Dispose ()

//  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
//  let cmd () = new CmdBuilder()
 
//  let cd' = Command "cd"
//  let dir' = Command "dir"