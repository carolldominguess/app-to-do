using App.ToDo.Application.Requests;
using App.ToDo.Domain.Interfaces;
using App.ToDo.Domain.Interfaces.Repositories;

namespace App.ToDo.Application.Handlers.Update;

public class UpdatePersistenceHandler : Handler<UpdateUcRequest>
{
    private readonly IToDoTaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePersistenceHandler(IToDoTaskRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    protected override void Handle(UpdateUcRequest request)
    {
        var entity = _repository.GetById(request.Id);

        if (entity is null)
        {
            request.AddError($"Tarefa com Id '{request.Id}' não encontrada.");
            return;
        }

        entity.Update(request.Title, request.Description, request.DueDate, request.Status);
        _repository.Update(entity);
        _unitOfWork.Commit();
    }
}
