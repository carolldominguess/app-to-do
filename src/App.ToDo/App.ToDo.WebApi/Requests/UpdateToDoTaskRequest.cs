using App.ToDo.Domain.Enums;

namespace App.ToDo.WebApi.Requests;

public class UpdateToDoTaskRequest
{
    /// <summary>Título da tarefa.</summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>Descrição detalhada da tarefa.</summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>Novo status da tarefa.</summary>
    public ToDoStatus Status { get; init; }

    /// <summary>Nova data de vencimento da tarefa.</summary>
    public DateTime DueDate { get; init; }
}
