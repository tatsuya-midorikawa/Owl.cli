![img](https://raw.githubusercontent.com/tatsuya-midorikawa/Owl.cli/main/assets/owl-console.png) 

# 🔷 Owl.cli

Owl.cli is a library to facilitate the use of shell from F#.

## 🔹 Usage

### 1️⃣ *cmd* computation expression

#### ▫️ `exec` custom op : string -> string

Execute the command passed to `exec` and receive the result.
Use `into` to receive the results.

```fsharp
open Owl.cli.cmd

use c = cmd () {
  exec @"dir C:\" into r
  printfn $"%s{r}"
}
```

#### ▫️ `exit` custom op : unit -> ShellBuilder

Explicitly terminates the cmd.
If `exit` is not called, the cmd also exits when the ShellBuilder is disposed.

```fsharp
open Owl.cli.cmd

use c = cmd () {
  exec @"dir C:\"
  exit
}
```

#### ▫️ `Results` property : array<Output>

Obtain a pair of executed commands and their results.

```fsharp
open Owl.cli.cmd

use c = cmd () {
  exec @"dir C:\"
  exit
}

c.Results |> Array.iter (printfn "%A")
```

### 2️⃣ *powershell* computation expression

#### ▫️ `exec` custom op : string -> string

Execute the command passed to `exec` and receive the result.
Use `into` to receive the results.

```fsharp
open Owl.cli.powershell

use p = powershell () {
  exec @"ls C:\" into r
  printfn $"%s{r}"
}
```

#### ▫️ `exit` custom op : unit -> ShellBuilder

Explicitly terminates the powershell.
If `exit` is not called, the powershell also exits when the ShellBuilder is disposed.

```fsharp
open Owl.cli.powershell

use p = powershell () {
  exec @"ls C:\"
  exit
}
```

#### ▫️ `Results` property : array<Output>

Obtain a pair of executed commands and their results.

```fsharp
open Owl.cli.powershell

use p = powershell () {
  exec @"ls C:\"
  exit
}

p.Results |> Array.iter (printfn "%A")
```