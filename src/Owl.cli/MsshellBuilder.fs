﻿namespace Owl.cli

open System
open System.Diagnostics
open System.Text
open Owl.cli.common

[<AbstractClass>]
type MsshellBuilder (psi: ProcessStartInfo) =
  let eoc' = "echo \"Owl.cli.console: End of command\""
  let output' = ResizeArray<Output>()
  let prc' = Process.Start psi
  let mutable state' = Stop
  do
    state' <- Running
    prc'.StandardInput.WriteLine(eoc')
    let mutable s = prc'.StandardOutput.ReadLine()
    while s <> Unchecked.defaultof<_> && not <| s.EndsWith eoc' do
      s <- prc'.StandardOutput.ReadLine()

  member __.Yield (x) = x
  member __.For (x, f) = f x
  member __.Zero () = __  

  member __.results with get () = output'.ToArray()
    
  [<CustomOperation("exec", AllowIntoPattern=true)>]
  member __.exec (v, [<ProjectionParameter>] cmd: unit -> string) =
    let cmd = cmd()
    let acc = StringBuilder()
    if state' = Running
      then           
        prc'.StandardInput.WriteLine (cmd)
        prc'.StandardInput.WriteLine (eoc')
        let mutable s = prc'.StandardOutput.ReadLine()

        // Discard command string (cmd) logic.
        while not <| s.EndsWith cmd do
          s <- prc'.StandardOutput.ReadLine()
        s <- prc'.StandardOutput.ReadLine()

        while s <> Unchecked.defaultof<_> && not <| s.EndsWith eoc' do
          acc.Append $"{s}{Environment.NewLine}" |> ignore
          s <- prc'.StandardOutput.ReadLine()
        prc'.StandardOutput.ReadLine() |> ignore // Discard command string (echo).
        output'.Add { cmd = cmd; result = acc.ToString() }
    acc.ToString()

  [<CustomOperation("exit")>]
  member __.exit (state: obj) =
    let ret = __.exec (state, fun () -> "exit")
    state' <- Stop;
    __

  interface IDisposable with
    member __.Dispose() = __.exit(__) |> ignore; prc'.Dispose ()