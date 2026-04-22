using App.ToDo.Domain.Enums;

namespace App.ToDo.WebApi.Requests;

public class CreateToDoTaskRequest
{
    /// <summary>Título da tarefa.</summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>Descrição detalhada da tarefa.</summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>Status inicial da tarefa.</summary>
    public ToDoStatus Status { get; init; } = ToDoStatus.Pending;

    /// <summary>Data de vencimento da tarefa.</summary>
    public DateTime DueDate { get; init; }
}
