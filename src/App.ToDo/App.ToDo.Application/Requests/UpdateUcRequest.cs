using App.ToDo.Domain.Enums;

namespace App.ToDo.Application.Requests;

public class UpdateUcRequest : Request
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public ToDoStatus Status { get; init; }
    public DateTime DueDate { get; init; }
}
