using App.ToDo.Domain.Enums;

namespace App.ToDo.WebApi.Responses;

public class ToDoTaskResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public ToDoStatus Status { get; init; }
    public string StatusDescription => Status switch
    {
        ToDoStatus.Pending => "Pendente",
        ToDoStatus.InProgress => "Em andamento",
        ToDoStatus.Completed => "Concluído",
        _ => "Desconhecido"
    };
    public DateTime DueDate { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
