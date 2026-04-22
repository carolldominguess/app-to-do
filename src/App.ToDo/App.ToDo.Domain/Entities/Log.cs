using App.ToDo.Domain.Enums;

namespace App.ToDo.Domain.Entities;

public class Log
{
    public Guid Id { get; private set; }

    /// <summary>Nome do UseCase ou Handler que originou o log.</summary>
    public string Source { get; private set; } = string.Empty;

    public LogStatus Status { get; private set; }

    /// <summary>Mensagem de sucesso ou descrição do erro.</summary>
    public string? Message { get; private set; }

    public DateTime CreatedAt { get; private set; }

    protected Log() { }

    public Log(string source, LogStatus status, string? message = null)
    {
        Id = Guid.NewGuid();
        Source = source;
        Status = status;
        Message = message;
        CreatedAt = DateTime.UtcNow;
    }
}
