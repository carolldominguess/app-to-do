using App.ToDo.Domain.Enums;

namespace App.ToDo.Infra.Entities;
public class ToDoTaskEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ToDoStatus Status { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}