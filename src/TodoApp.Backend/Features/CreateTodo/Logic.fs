module TodoApp.Backend.Features.CreateTodo.Logic

open TodoApp.Shared
open TodoApp.Shared.Features.CreateTodo
open System

let validate (cmd: Command) (now: DateTime) : Result<Todo, CreateError> =
    let titleStr = cmd.Title.Trim()

    if String.IsNullOrWhiteSpace(titleStr) then
        Error TitleEmpty
    elif titleStr.Length > 200 then
        Error (TitleTooLong 200)
    else
        Ok {
            Id = TodoId cmd.Id
            Title = TodoTitle titleStr
            Status = New
            DueDate = cmd.DueDate
            CreatedAt = now
        }
