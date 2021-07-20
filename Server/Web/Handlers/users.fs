module Todo.Handlers.Users

open System
open FSharp.Control.Tasks
open System.Linq
open FSharp.Data
open Giraffe

let userRegister: HttpHandler = 
  handleContext(
    fun ctx ->
      task{
        let! (user:Todo.Util.Types.User) = ctx.BindModelAsync<Todo.Util.Types.User>()
        sprintf "printing stuff %s" user.First_name |> ignore<string>
       // return! ctx.WriteTextAsync user.First_name
        let db = ctx.GetService<Todo.Util.DB.IConnectionFactory>()
        let! res = db.WithConnection <| fun conn -> async {
          let! storedUser = Todo.API.Context.User.storeUser conn user 
          return storedUser
        }
         return! ctx.WriteJsonAsync user
        }) 