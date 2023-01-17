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

  //let internal chain (cmds: seq<string>) = [| cmds |> String.concat " >> " |]
  //let internal chains (cmds: seq<string[]>) = cmds |> Seq.map (String.concat " >> ")

  type state = Stop | Running
  type command = Command of string

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
      if state' = Running then
        __.exit prc' |> ignore
      task { do! prc'.WaitForExitAsync () } |> Task.WaitAll
      stdout'.ToString()

    [<CustomOperation("exec")>]
    member __.exec (_, cmd: string) =
      task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll; __;
    [<CustomOperation("exec")>]
    member __.exec (_, Command cmd) =
      task { do! prc'.StandardInput.WriteLineAsync cmd } |> Task.WaitAll; __;


    [<CustomOperation("exit")>]
    member __.exit (_: obj) =
      task { do! prc'.StandardInput.WriteLineAsync "exit" } |> Task.WaitAll
      state' <- Stop
      __

    interface IDisposable with
      member __.Dispose() = prc'.Dispose ()

  [<System.Runtime.Versioning.SupportedOSPlatform("Windows")>]
  let cmd = new CmdBuilder()
  
  // https://learn.microsoft.com/ja-jp/windows-server/administration/windows-commands/windows-commands?source=recommendations
  (* ///------> *)
  // === A ===

  // === B ===

  // === C ===
  let inline cd path = Command $"cd \"{path}\""
  let cls = Command $"cls"

  // === D ===
  let dir = Command $"dir"
  let inline dir' path = Command $"dir \"{path}\""

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
  let systeminfo = Command $"systeminfo"

  // === T ===
  // === U ===
  // === V ===
  // === W ===
  // === X ===
  // === Y ===
  // === Z ===
  (* <------/// *)

  
  // https://stackoverflow.com/questions/28889954/what-does-do-in-this-batch-file
  (* ///------> *)
  // output to a file
  let inline (.>) (Command fst) (Command snd) = Command $"%s{fst} > %s{snd}"
  // append output to a file
  let inline (.>>) (Command fst) (Command snd) = Command $"%s{fst} >> %s{snd}"
  // separates commands on a line.
  let inline (<&>) (Command fst) (Command snd) = Command $"%s{fst} & %s{snd}"
  // executes this command only if previous command's errorlevel is 0.
  let inline (<&&>) (Command fst) (Command snd) = Command $"%s{fst} && %s{snd}"
  // input from a file
  let inline (<.) (Command fst) (Command snd) = Command $"%s{fst} && %s{snd}"

  (* <------/// *)