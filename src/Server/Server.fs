module Server
open Saturn
open Giraffe

let port =
    match System.Environment.GetEnvironmentVariable("PORT") with
    | null -> uint16 8088
    | port -> uint16 port
let publicPath = System.IO.Path.GetFullPath "./public"

let app =
  application {
    use_static publicPath
    use_router (text "Hello World from Saturn")
#if !DEBUG
    disable_diagnostics
#endif
    url ("http://0.0.0.0:" + port.ToString() + "/")
  }

run app
