using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Enums;

namespace App.ToDo.UnitTests.Builders;

public class ToDoTaskBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _title = "Tarefa de teste";
    private string _description = "Descrição da tarefa de teste";
    private ToDoStatus _status = ToDoStatus.Pending;
    private DateTime _dueDate = DateTime.UtcNow.AddDays(7);

    public static ToDoTaskBuilder New() => new();

    public ToDoTaskBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public ToDoTaskBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public ToDoTaskBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ToDoTaskBuilder WithStatus(ToDoStatus status)
    {
        _status = status;
        return this;
    }

    public ToDoTaskBuilder WithDueDate(DateTime dueDate)
    {
        _dueDate = dueDate;
        return this;
    }

    public ToDoTask Build()
    {
        var entity = new ToDoTask(_title, _description, _dueDate, _status);

        var idProp = typeof(ToDoTask).GetProperty(nameof(ToDoTask.Id))!;
        idProp.SetValue(entity, _id);

        return entity;
    }
}
