using App.ToDo.Application.Requests;
using App.ToDo.Domain.Enums;

namespace App.ToDo.UnitTests.Builders;

public class AddUcRequestBuilder
{
    private string _title = "Nova tarefa";
    private string _description = "Descrição válida para a tarefa";
    private ToDoStatus _status = ToDoStatus.Pending;
    private DateTime _dueDate = DateTime.UtcNow.AddDays(5);

    public static AddUcRequestBuilder New() => new();

    public AddUcRequestBuilder WithTitle(string title) { _title = title; return this; }
    public AddUcRequestBuilder WithDescription(string description) { _description = description; return this; }
    public AddUcRequestBuilder WithStatus(ToDoStatus status) { _status = status; return this; }
    public AddUcRequestBuilder WithDueDate(DateTime dueDate) { _dueDate = dueDate; return this; }

    public AddUcRequest Build() => new()
    {
        Title = _title,
        Description = _description,
        Status = _status,
        DueDate = _dueDate
    };
}
