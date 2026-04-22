using App.ToDo.Domain.Enums;

namespace App.ToDo.WebApi.Requests;

public class SearchToDoTaskRequest
{
    /// <summary>Filtrar por título (contém).</summary>
    public string? Title { get; init; }

    /// <summary>Filtrar por status.</summary>
    public ToDoStatus? Status { get; init; }

    /// <summary>Data de vencimento inicial (inclusive).</summary>
    public DateTime? DueDateFrom { get; init; }

    /// <summary>Data de vencimento final (inclusive).</summary>
    public DateTime? DueDateTo { get; init; }

    /// <summary>Número da página (padrão: 1).</summary>
    public int Page { get; init; } = 1;

    /// <summary>Tamanho da página (padrão: 10, máximo: 100).</summary>
    public int PageSize { get; init; } = 10;
}
