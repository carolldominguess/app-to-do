using App.ToDo.Domain.Enums;

namespace App.ToDo.Application.Requests;

public class SearchUcRequest : Request
{
    public string? Title { get; init; }
    public ToDoStatus? Status { get; init; }
    public DateTime? DueDateFrom { get; init; }
    public DateTime? DueDateTo { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
