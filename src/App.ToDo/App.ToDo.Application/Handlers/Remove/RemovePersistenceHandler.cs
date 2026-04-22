using App.ToDo.Application.Requests;
using App.ToDo.Domain.Interfaces;
using App.ToDo.Domain.Interfaces.Repositories;

namespace App.ToDo.Application.Handlers.Remove;

public class RemovePersistenceHandler : Handler<RemoveUcRequest>
{
    private readonly IToDoTaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RemovePersistenceHandler(IToDoTaskRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    protected override void Handle(RemoveUcRequest request)
    {
        var entity = _repository.GetById(request.Id);

        if (entity is null)
        {
            request.AddError($"Tarefa com Id '{request.Id}' não encontrada.");
            return;
        }

        _repository.Remove(entity);
        _unitOfWork.Commit();
    }
}
