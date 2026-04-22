using App.ToDo.Domain.Enums;

namespace App.ToDo.Domain.Entities;

public class ToDoTask
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ToDoStatus Status { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected ToDoTask() { }

    public ToDoTask(string title, string description, DateTime dueDate, ToDoStatus status = ToDoStatus.Pending)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        DueDate = dueDate;
        Status = status;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string description, DateTime dueDate, ToDoStatus status)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}