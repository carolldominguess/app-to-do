using App.ToDo.Domain.Enums;

namespace App.ToDo.Infra.Entities;

public class LogEntity
{
    public Guid Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public LogStatus Status { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
}
