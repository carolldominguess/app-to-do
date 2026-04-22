using App.ToDo.Application.Requests;
using App.ToDo.Domain.Interfaces.Repositories;

namespace App.ToDo.Application.Handlers.Remove;

public class RemoveValidationHandler : Handler<RemoveUcRequest>
{
    private readonly IToDoTaskRepository _repository;

    public RemoveValidationHandler(IToDoTaskRepository repository)
    {
        _repository = repository;
    }

    protected override void Handle(RemoveUcRequest request)
    {
        var entity = _repository.GetById(request.Id);

        if (entity is null)
            request.AddError($"Tarefa com Id '{request.Id}' não encontrada.");
    }
}
