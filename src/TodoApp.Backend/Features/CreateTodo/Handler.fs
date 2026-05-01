module TodoApp.Backend.Features.CreateTodo.Handler

open Microsoft.AspNetCore.Http
open Giraffe
open TodoApp.Shared.Features.CreateTodo
open System

let insertIntoDb (todo: Todo) = task { return () } // Dummy

let handleCreateTodo : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! cmd = ctx.BindJsonAsync<Command>()
            match Logic.validate cmd DateTime.UtcNow with
            | Ok todo ->
                do! insertIntoDb todo
                ctx.SetStatusCode 201
                return! json todo next ctx
            | Error TitleEmpty ->
                ctx.SetStatusCode 400
                return! text "Titel darf nicht leer sein" next ctx
            | Error (TitleTooLong max) ->
                ctx.SetStatusCode 400
                return! text $"Titel darf maximal {max} Zeichen lang sein" next ctx
        }
