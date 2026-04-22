using App.ToDo.Application.Requests;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Interfaces;
using App.ToDo.Domain.Interfaces.Repositories;

namespace App.ToDo.Application.Handlers.Add;

public class AddPersistenceHandler : Handler<AddUcRequest>
{
    private readonly IToDoTaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPersistenceHandler(IToDoTaskRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    protected override void Handle(AddUcRequest request)
    {
        var entity = new ToDoTask(request.Title, request.Description, request.DueDate, request.Status);
        _repository.Add(entity);
        _unitOfWork.Commit();
    }
}
