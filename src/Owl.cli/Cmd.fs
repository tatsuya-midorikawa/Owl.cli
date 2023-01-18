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

  // https://stackoverflow.com/questions/28889954/what-does-do-in-this-batch-file
  (* ///------> *)
  type op = string -> string option -> string
  type op2 = string -> command option -> string
  let (.>) : op = fun lhs -> function Some rhs -> $"%s{lhs} > %s{rhs}" | None -> lhs
  let (.>>) : op = fun lhs -> function Some rhs -> $"%s{lhs} >> %s{rhs}" | None -> lhs
  let (<&>) : op2 = fun lhs -> function Some (Command rhs) -> $"%s{lhs} & %s{rhs}" | None -> lhs
  let (<&&>) : op2 = fun lhs -> function Some (Command rhs) -> $"%s{lhs} && %s{rhs}" | None -> lhs
  (* <------/// *)

  let build_op (cmd: string) (dst: string option) (op: op option) =
    match op with Some op -> op cmd dst | None -> cmd
  let build_op2 (cmd1: string) (cmd2: command option) (op: op2 option) =
    match op with Some op -> op cmd1 cmd2 | None -> cmd1

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
    member __.cd (_, path, ?op: op2, ?cmd2: command, ?args: seq<string>) =
      let cmd = op |> build_op2 $"cd %s{path}" (combine cmd2 args)
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __

    [<CustomOperation("cls")>]
    member __.cls (_, ?op: op2, ?cmd2: command, ?args: seq<string>) =
      let cmd = op |> build_op2 $"cls" (combine cmd2 args)
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __

    // === D ===
    [<CustomOperation("dir")>]
    member __.dir (_, ?path: string, ?op: op, ?dst: string) =
      let dir = match path with Some p -> $"dir {p}" | None -> "dir"
      let cmd = op |> build_op dir dst
      if state' = Running
      then task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll
      __
    [<CustomOperation("dir")>]
    member __.dir (_, ?path: string, ?op: op2, ?cmd2: command, ?args: seq<string>) =
      let dir = match path with Some p -> $"dir {p}" | None -> "dir"
      let cmd = op |> build_op2 dir (combine cmd2 args)
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
    // === S ===
    [<CustomOperation("systeminfo")>]
    member __.systeminfo (_, ?op: op, ?dst: string) =
      let cmd = op |> build_op $"systeminfo" dst
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