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

use builder = cmd () {
  exec @"dir C:\" into r
  printfn $"%s{r}"
}
```

#### ▫️ `exit` custom op : unit -> ShellBuilder

Explicitly terminates the cmd.
If `exit` is not called, the cmd also exits when the ShellBuilder is disposed.

```fsharp
open Owl.cli.cmd

use builder = cmd () {
  exec @"dir C:\"
  exit
}
```

#### ▫️ `results` property : array&lt;Output&gt;

Obtain a pair of executed commands and their results.

```fsharp
open Owl.cli.cmd

use builder = cmd () {
  exec @"dir C:\"
  exit
}

builder.results |> Array.iter (printfn "%A")
```

### 2️⃣ *powershell* computation expression (LEGACY PowerShell)

#### ▫️ `exec` custom op : string -> string

Execute the command passed to `exec` and receive the result.
Use `into` to receive the results

```fsharp
open Owl.cli.powershell

use builder = powershell () {
  exec @"ls C:\" into r
  printfn $"%s{r}"
}
```

#### ▫️ `exit` custom op : unit -> ShellBuilder

Explicitly terminates the powershell.
If `exit` is not called, the powershell also exits when the ShellBuilder is disposed.

```fsharp
open Owl.cli.powershell

use builder = powershell () {
  exec @"ls C:\"
  exit
}
```

#### ▫️ `results` property : array&lt;Output&gt;

Obtain a pair of executed commands and their results.

```fsharp
open Owl.cli.powershell

use builder = powershell () {
  exec @"ls C:\"
  exit
}

builder.results |> Array.iter (printfn "%A")
```

### 3️⃣ *pwsh* computation expression (PowerShell Core)

#### ▫️ `exec` custom op : string -> string

Execute the command passed to `exec` and receive the result.
Use `into` to receive the results.

```fsharp
open Owl.cli.pwsh

use builder = pwsh () {
  exec @"ls /bin" into r
  printfn $"%s{r}"
}
```

#### ▫️ `exit` custom op : unit -> ShellBuilder

Explicitly terminates the pwsh.
If `exit` is not called, the pwsh also exits when the ShellBuilder is disposed.

```fsharp
open Owl.cli.pwsh

use builder = pwsh () {
  exec @"ls /bin"
  exit
}
```

#### ▫️ `results` property : array&lt;Output&gt;

Obtain a pair of executed commands and their results.

```fsharp
open Owl.cli.pwsh

use builder = pwsh () {
  exec @"ls /bin"
  exit
}

builder.results |> Array.iter (printfn "%A")
```

### 4️⃣ *zsh* computation expression

#### ▫️ `exec` custom op : string -> string

Execute the command passed to `exec` and receive the result.
Use `into` to receive the results.

```fsharp
open Owl.cli.zsh

use builder = zsh () {
  exec @"ls /bin" into r
  printfn $"%s{r}"
}
```

#### ▫️ `exit` custom op : unit -> ShellBuilder

Explicitly terminates the zsh.
If `exit` is not called, the zsh also exits when the ShellBuilder is disposed.

```fsharp
open Owl.cli.zsh

use builder = zsh () {
  exec @"ls /bin"
  exit
}
```

#### ▫️ `results` property : array&lt;Output&gt;

Obtain a pair of executed commands and their results.

```fsharp
open Owl.cli.zsh

use builder = zsh () {
  exec @"ls /bin"
  exit
}

builder.results |> Array.iter (printfn "%A")
```

### 5️⃣ *bash* computation expression

#### ▫️ `exec` custom op : string -> string

Execute the command passed to `exec` and receive the result.
Use `into` to receive the results.

```fsharp
open Owl.cli.bash

use builder = bash () {
  exec @"ls /bin" into r
  printfn $"%s{r}"
}
```

#### ▫️ `exit` custom op : unit -> ShellBuilder

Explicitly terminates the bash.
If `exit` is not called, the bash also exits when the ShellBuilder is disposed.

```fsharp
open Owl.cli.bash

use builder = bash () {
  exec @"ls /bin"
  exit
}
```

#### ▫️ `results` property : array&lt;Output&gt;

Obtain a pair of executed commands and their results.

```fsharp
open Owl.cli.bash

use builder = bash () {
  exec @"ls /bin"
  exit
}

builder.results |> Array.iter (printfn "%A")
```

### 6️⃣ *cli* computation expression

> **Warning**
> `cli` will be a generic implementation of CE. 
> Some shells such as 'cmd' will not be handled correctly because the base of behavior will be 'zsh' or 'bash' equivalent.

#### ▫️ `exec` custom op : string -> string

Execute the command passed to `exec` and receive the result.
Use `into` to receive the results.

```fsharp
open Owl.cli.general

use builder = cli "/bin/zsh" {
  exec @"ls /bin" into r
  printfn $"%s{r}"
}
```

#### ▫️ `exit` custom op : unit -> ShellBuilder

Explicitly terminates the cli.
If `exit` is not called, the cli also exits when the ShellBuilder is disposed.

```fsharp
open Owl.cli.general

use builder = cli () {
  exec @"ls /bin"
  exit
}
```

#### ▫️ `results` property : array&lt;Output&gt;

Obtain a pair of executed commands and their results.

```fsharp
open Owl.cli.general

use builder = cli () {
  exec @"ls /bin"
  exit
}

builder.results |> Array.iter (printfn "%A")
```