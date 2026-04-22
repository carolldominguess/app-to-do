using App.ToDo.Domain.Enums;

namespace App.ToDo.Application.Requests;

public class AddUcRequest : Request
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public ToDoStatus Status { get; init; } = ToDoStatus.Pending;
    public DateTime DueDate { get; init; }
}
