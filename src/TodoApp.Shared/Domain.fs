namespace TodoApp.Shared

open System

type TodoId = TodoId of Guid
type TodoTitle = TodoTitle of string

type Status =
    | New
    | Active
    | Paused
    | Completed
