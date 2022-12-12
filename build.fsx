#r "paket: groupref build //"
#load "./.fake/build.fsx/intellisense.fsx"
#r "netstandard"

open Fake.Core
open Fake.DotNet
open Fake.IO

Target.initEnvironment ()

let serverPath = Path.getFullName "./src/Server"
let deployDir = Path.getFullName "./deploy"

let dotnet cmd workingDir =
    let result = DotNet.exec (DotNet.Options.withWorkingDirectory workingDir) cmd ""
    if result.ExitCode <> 0 then failwithf "'dotnet %s' failed in %s" cmd workingDir

Target.create "Clean" (fun _ -> Shell.cleanDir deployDir)

let bundle () =
    let runtimeOption = if Environment.isUnix then "--runtime linux-x64" else ""
    dotnet (sprintf "publish -c Release -o \"%s\" %s" deployDir runtimeOption) serverPath

Target.create "Bundle" (fun _ -> bundle())

open Fake.Core.TargetOperators

"Clean"
    ==> "Bundle"

Target.runOrDefaultWithArguments "Bundle"
