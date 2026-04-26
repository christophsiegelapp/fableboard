module TodoApp.Shared.Features.CreateTodo

open System
open TodoApp.Shared

// Command payload from client
type Command = {
    Id: Guid
    Title: string
    DueDate: DateTime option
}

// Resulting entity
type Todo = {
    Id: TodoId
    Title: TodoTitle
    Status: Status
    DueDate: DateTime option
    CreatedAt: DateTime
}

// Error types
type CreateError =
    | TitleEmpty
    | TitleTooLong of maxChars: int
