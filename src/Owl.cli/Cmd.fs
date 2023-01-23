namespace owl.cli

//[<AutoOpen>]
module cmd =
  open System
  open System.Diagnostics
  open System.Text
  open System.Threading.Tasks

  let private cmd' = Environment.GetEnvironmentVariable "ComSpec"
  let private psi' = ProcessStartInfo (cmd', 
    // enable commnads input and reading of output
    UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true,
    // hide console window
    WindowStyle = ProcessWindowStyle.Hidden, CreateNoWindow = true)

  type state = Stop | Running
  type command = Command of string

  let combine cmd args =
    let args = match args with Some args -> args |> String.concat " " | None -> ""
    cmd |> Option.map (fun (Command cmd) -> Command $"%s{cmd} %s{args}")

  let escape (s: string) = 
    if not(s.StartsWith('\"')) then $"\"%s{s}" else s
    |> (fun s -> if not(s.EndsWith('\"')) then $"%s{s}\"" else s)
    

  // https://stackoverflow.com/questions/28889954/what-does-do-in-this-batch-file
  (* ///------> *)
  type op_str = string -> string option -> string
  type op_cmd = string -> command option -> string
  let build_op_str (cmd: string) (dst: string option) (op: op_str option) =
    match op with Some op -> op cmd dst | None -> cmd
  let build_op_cmd (cmd1: string) (cmd2: command option) (op: op_cmd option) =
    match op with Some op -> op cmd1 cmd2 | None -> cmd1

  let (.>) : op_str = fun lhs -> function Some rhs -> $"%s{lhs} > %s{rhs}" | None -> lhs
  let (.>>) : op_str = fun lhs -> function Some rhs -> $"%s{lhs} >> %s{rhs}" | None -> lhs
  let (<&>) : op_cmd = fun lhs -> function Some (Command rhs) -> $"%s{lhs} & %s{rhs}" | None -> lhs
  let (<&&>) : op_cmd = fun lhs -> function Some (Command rhs) -> $"%s{lhs} && %s{rhs}" | None -> lhs
  (* <------/// *)

  type RegType = 
    | REG_SZ = 0 
    | REG_MULTI_SZ = 1 
    | REG_DWORD_BIG_ENDIAN = 2 
    | REG_DWORD = 3
    | REG_BINARY = 4
    | REG_DWORD_LITTLE_ENDIAN = 5
    | REG_LINK = 6
    | REG_FULL_RESOURCE_DESCRIPTOR = 7
    | REG_EXPAND_SZ = 8

  type AddCmd = AddCmd of string
  type CompareCmd = CompareCmd of string
  type CopyCmd = CopyCmd of string
  type DeleteCmd = DeleteCmd of string
  type ExportCmd = ExportCmd of string
  type ImportCmd = ImportCmd of string
  type LoadCmd = LoadCmd of string
  type QueryCmd = QueryCmd of string
  type RestoreCmd = RestoreCmd of string
  type SaveCmd = SaveCmd of string
  type UnloadCmd = UnloadCmd of string

  let add = AddCmd "add"
  let compare = CompareCmd "compare"
  let copy = CopyCmd "copy"
  let delete = DeleteCmd "delete"
  let export = ExportCmd "export"
  let import = ImportCmd "import"
  let load = LoadCmd "load"
  let query = QueryCmd "query"
  let restore = RestoreCmd "restore"
  let save = SaveCmd "save"
  let unload = UnloadCmd "unload"

  // https://learn.microsoft.com/ja-jp/windows-server/administration/windows-commands/windows-commands?source=recommendations
  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  type CmdBuilder () =
    let ignore' = [| "\f"; "\r"; "\n";  "\r\n"; "";  |]
    let prc' = Process.Start psi'
    let stdout' = StringBuilder()
    let mutable state' = Stop
    let mutable cnt = 0L
    do
      state' <- Running
      prc'.OutputDataReceived.Add (fun e ->
        if 3L < cnt && ignore'|> Array.contains e.Data |> not && e.Data <> null then 
          stdout'.AppendLine e.Data |> ignore
        cnt <- cnt + 1L
      )
      prc'.BeginOutputReadLine()

    member __.Yield (_) = __
    member __.Result () =
      if state' = Running
      then __.exit prc' |> ignore
      task { do! prc'.WaitForExitAsync () } |> Task.WaitAll
      stdout'.ToString()

    [<CustomOperation("exec")>]
    member __.exec (_, cmd: string) =
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __

    [<CustomOperation("exec")>]
    member __.exec (_, Command cmd) =
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __

    [<CustomOperation("exit")>]
    member __.exit (_: obj) =
      if state' = Running then
        task { do! prc'.StandardInput.WriteLineAsync "exit" } |> Task.WaitAll
        state' <- Stop
      __
      
    // === A ===

    // === B ===
    // === C ===
    [<CustomOperation("cd")>]
    member __.cd (_, path, ?op: op_cmd, ?cmd2: command, ?args: string list) =
      let cmd = op |> build_op_cmd $"cd %s{path}" (combine cmd2 args)
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __

    [<CustomOperation("cls")>]
    member __.cls (_, ?op: op_cmd, ?cmd2: command, ?args: string list) =
      let cmd = op |> build_op_cmd $"cls" (combine cmd2 args)
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __

    // === D ===
    [<CustomOperation("dir")>]
    member __.dir (_, ?path: string, ?op: op_str, ?dst: string) =
      let dir = match path with Some p -> $"dir {p}" | None -> "dir"
      let cmd = op |> build_op_str dir dst
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __
    [<CustomOperation("dir")>]
    member __.dir (_, ?path: string, ?op: op_cmd, ?cmd2: command, ?args: string list) =
      let dir = match path with Some p -> $"dir {p}" | None -> "dir"
      let cmd = op |> build_op_cmd dir (combine cmd2 args)
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __

    // === E ===
    // === F ===
    // === G ===
    // === H ===
    // === I ===
    // === J ===
    // === K ===
    // === L ===
    // === M ===
    // === N ===
    // === O ===
    // === P ===
    // === Q ===
    // === R ===
    [<CustomOperation("reg")>]
    member __.reg (_, AddCmd addcmd, key, ?args) =
      let args = match args with Some args -> args |> Seq.reduce (fun a b -> $"%s{a} %s{b}") | _ -> ""
      let cmd = $"reg %s{addcmd} \"%s{key}\" %s{args}"
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __
    [<CustomOperation("reg")>]
    member __.reg (_, CompareCmd comparecmd, key1, key2, ?args) =
      let args = match args with Some args -> args |> Seq.reduce (fun a b -> $"%s{a} %s{b}") | _ -> ""
      let cmd = $"reg %s{comparecmd} \"%s{key1}\" \"%s{key2}\" %s{args}"
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __
    [<CustomOperation("reg")>]
    member __.reg (_, CopyCmd copycmd, key1, key2, ?args) =
      let args = match args with Some args -> args |> Seq.reduce (fun a b -> $"%s{a} %s{b}") | _ -> ""
      let cmd = $"reg %s{copycmd} \"%s{key1}\" \"%s{key2}\" %s{args}"
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __
    [<CustomOperation("reg")>]
    member __.reg (_, DeleteCmd deletecmd, key, ?args) =
      let args = match args with Some args -> args |> Seq.reduce (fun a b -> $"%s{a} %s{b}") | _ -> ""
      let cmd = $"reg %s{deletecmd} \"%s{key}\" %s{args}"
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __
    [<CustomOperation("reg")>]
    member __.reg (_, ExportCmd exportcmd, key, filename, ?args) =
      let args = match args with Some args -> args |> Seq.reduce (fun a b -> $"%s{a} %s{b}") | _ -> ""
      let cmd = $"reg %s{exportcmd} \"%s{key}\" \"%s{filename}\" %s{args}"
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __
    [<CustomOperation("reg")>]
    member __.reg (_, ImportCmd importcmd, filename, ?args) =
      let args = match args with Some args -> args |> Seq.reduce (fun a b -> $"%s{a} %s{b}") | _ -> ""
      let cmd = $"reg %s{importcmd} \"%s{filename}\" %s{args}"
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __
    [<CustomOperation("reg")>]
    member __.reg (_, LoadCmd loadcmd, key, filename) =
      let cmd = $"reg %s{loadcmd} \"%s{key}\" \"%s{filename}\""
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __
    [<CustomOperation("reg")>]
    member __.reg (_, RestoreCmd restorecmd, key, filename) =
      let cmd = $"reg %s{restorecmd} \"%s{key}\" \"%s{filename}\""
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __
    [<CustomOperation("reg")>]
    member __.reg (_, QueryCmd querycmd, key, ?args) =
      let args = match args with Some args -> args |> Seq.reduce (fun a b -> $"%s{a} %s{b}") | _ -> ""
      let cmd = $"reg %s{querycmd} \"%s{key}\" %s{args}"
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __
    [<CustomOperation("reg")>]
    member __.reg (_, SaveCmd savecmd, key, filename, ?args) =
      let args = match args with Some args -> args |> Seq.reduce (fun a b -> $"%s{a} %s{b}") | _ -> ""
      let cmd = $"reg %s{savecmd} \"%s{key}\" \"%s{filename}\" %s{args}"
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __
    [<CustomOperation("reg")>]
    member __.reg (_, UnloadCmd unloadcmd, key) =
      let cmd = $"reg %s{unloadcmd} \"%s{key}\""
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __

    // === S ===
    [<CustomOperation("systeminfo")>]
    member __.systeminfo (_, ?op: op_str, ?dst: string) =
      let cmd = op |> build_op_str $"systeminfo" dst
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __

    // === T ===
    // === U ===
    // === V ===
    // === W ===
    // === X ===
    // === Y ===
    // === Z ===

    interface IDisposable with
      member __.Dispose() = prc'.Dispose ()

  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  let cmd () = new CmdBuilder()
 
  let cd' = Command "cd"
  let dir' = Command "dir"