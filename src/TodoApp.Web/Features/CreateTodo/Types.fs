module TodoApp.Web.Features.CreateTodo.Types

open TodoApp.Shared.Features.CreateTodo

type Model = {
    DraftTitle: string
    IsSubmitting: bool
    ErrorMessage: string option
}

type Msg =
    | TitleChanged of string
    | SubmitClicked
    | ServerResponded of Result<Todo, string>
