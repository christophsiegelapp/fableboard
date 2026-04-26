module TodoApp.Mobile.Features.CreateTodo.State

open Elmish
open TodoApp.Shared.Features.CreateTodo
open TodoApp.Mobile.Features.CreateTodo.Types
open System

let init () =
    { DraftTitle = ""; IsSubmitting = false; ErrorMessage = None }, Cmd.none

let update (apiPost: Command -> Async<Result<Todo, string>>) (msg: Msg) (model: Model) =
    match msg with
    | TitleChanged newTitle ->
        { model with DraftTitle = newTitle; ErrorMessage = None }, Cmd.none

    | SubmitClicked ->
        if String.IsNullOrWhiteSpace(model.DraftTitle) then
            { model with ErrorMessage = Some "Titel darf nicht leer sein" }, Cmd.none
        else
            let cmd: Command = {
                Id = Guid.NewGuid()
                Title = model.DraftTitle
                DueDate = None
            }
            let effect = Cmd.OfAsync.perform apiPost cmd ServerResponded
            { model with IsSubmitting = true }, effect

    | ServerResponded (Ok _) ->
        { model with DraftTitle = ""; IsSubmitting = false }, Cmd.none

    | ServerResponded (Error err) ->
        { model with ErrorMessage = Some err; IsSubmitting = false }, Cmd.none
