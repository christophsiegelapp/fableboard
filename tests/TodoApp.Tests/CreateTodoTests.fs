module TodoApp.Tests.CreateTodoTests

open System
open Xunit
open FsUnit.Xunit

open TodoApp.Shared
open TodoApp.Shared.Features.CreateTodo
open TodoApp.Backend.Features.CreateTodo.Logic

// Tracer Bullet (Red -> Green -> Refactor) for validating a valid command

[<Fact>]
let ``Valid Command returns Ok Result with correct properties`` () =
    let id = Guid.NewGuid()
    let now = DateTime.UtcNow
    let cmd = { Id = id; Title = "Buy groceries"; DueDate = None }

    let result = validate cmd now

    match result with
    | Ok todo ->
        todo.Id |> should equal (TodoId id)
        todo.Title |> should equal (TodoTitle "Buy groceries")
        todo.Status |> should equal Status.New
        todo.DueDate |> should equal None
        todo.CreatedAt |> should equal now
    | Error _ -> failwith "Expected Ok, got Error"

[<Fact>]
let ``Title with only whitespaces returns TitleEmpty error`` () =
    let cmd = { Id = Guid.NewGuid(); Title = "   "; DueDate = None }

    let result = validate cmd DateTime.UtcNow

    match result with
    | Error CreateError.TitleEmpty -> Assert.True(true)
    | _ -> failwith $"Expected TitleEmpty error, but got {result}"

[<Fact>]
let ``Title longer than 200 characters returns TitleTooLong error`` () =
    let longTitle = String('A', 201)
    let cmd = { Id = Guid.NewGuid(); Title = longTitle; DueDate = None }

    let result = validate cmd DateTime.UtcNow

    match result with
    | Error (CreateError.TitleTooLong 200) -> Assert.True(true)
    | _ -> failwith $"Expected TitleTooLong error, but got {result}"

// TDD for MVU Update logic
module MVUTests =
    open TodoApp.Web.Features.CreateTodo.Types
    open TodoApp.Web.Features.CreateTodo.State

    // Stub api dependency
    let stubApiPost (cmd: Command) = async { return Ok { Id = TodoId cmd.Id; Title = TodoTitle cmd.Title; Status = Status.New; DueDate = None; CreatedAt = DateTime.UtcNow } }

    [<Fact>]
    let ``Init returns empty model and no command`` () =
        let model, _ = init ()
        model.DraftTitle |> should equal ""
        model.IsSubmitting |> should equal false
        model.ErrorMessage |> should equal None

    [<Fact>]
    let ``TitleChanged updates DraftTitle and clears ErrorMessage`` () =
        let initialModel = { DraftTitle = "Old"; IsSubmitting = false; ErrorMessage = Some "Error" }
        let model, _ = update stubApiPost (TitleChanged "New Title") initialModel

        model.DraftTitle |> should equal "New Title"
        model.ErrorMessage |> should equal None

    [<Fact>]
    let ``SubmitClicked with empty title sets ErrorMessage`` () =
        let initialModel = { DraftTitle = "  "; IsSubmitting = false; ErrorMessage = None }
        let model, _ = update stubApiPost SubmitClicked initialModel

        model.ErrorMessage |> should equal (Some "Titel darf nicht leer sein")
        model.IsSubmitting |> should equal false

    [<Fact>]
    let ``ServerResponded with Error sets ErrorMessage and stops submitting`` () =
        let initialModel = { DraftTitle = "Test"; IsSubmitting = true; ErrorMessage = None }
        let model, _ = update stubApiPost (ServerResponded (Error "Server kaputt")) initialModel

        model.ErrorMessage |> should equal (Some "Server kaputt")
        model.IsSubmitting |> should equal false
        model.DraftTitle |> should equal "Test" // Keeps draft
