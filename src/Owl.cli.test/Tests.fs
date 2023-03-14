module Tests

open System
open System.Diagnostics
open Xunit
open Owl.cli

[<Literal>]
let private zsh' = "/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal"

[<Fact>]
let ``My test`` () =
  let a = Zsh.exec()
  
  Assert.Equal("test", a)

[<Fact>]
let ``Zsh launch test`` () =

  // use p = 
  //   ProcessStartInfo (zsh',
  //     UseShellExecute = false,
  //     RedirectStandardInput = true,
  //     RedirectStandardOutput = true,
  //     CreateNoWindow = true)
  //   |> Process.Start

  use p = zsh' |> Process.Start
  Assert.True(false)