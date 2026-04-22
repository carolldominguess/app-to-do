using App.ToDo.Application.Requests;
using App.ToDo.Domain.Enums;

namespace App.ToDo.UnitTests.Builders;

public class UpdateUcRequestBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _title = "Tarefa atualizada";
    private string _description = "Descrição atualizada";
    private ToDoStatus _status = ToDoStatus.InProgress;
    private DateTime _dueDate = DateTime.UtcNow.AddDays(10);

    public static UpdateUcRequestBuilder New() => new();

    public UpdateUcRequestBuilder WithId(Guid id) { _id = id; return this; }
    public UpdateUcRequestBuilder WithTitle(string title) { _title = title; return this; }
    public UpdateUcRequestBuilder WithDescription(string description) { _description = description; return this; }
    public UpdateUcRequestBuilder WithStatus(ToDoStatus status) { _status = status; return this; }
    public UpdateUcRequestBuilder WithDueDate(DateTime dueDate) { _dueDate = dueDate; return this; }

    public UpdateUcRequest Build() => new()
    {
        Id = _id,
        Title = _title,
        Description = _description,
        Status = _status,
        DueDate = _dueDate
    };
}
