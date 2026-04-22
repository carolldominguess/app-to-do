using App.ToDo.Application.Handlers.Remove;
using App.ToDo.Application.Interfaces.UseCases;
using App.ToDo.Application.Requests;
using App.ToDo.Domain.Interfaces;
using App.ToDo.Domain.Interfaces.Repositories;

namespace App.ToDo.Application.UseCases;

public class RemoveUseCase : IRemoveUseCase
{
    private readonly IToDoTaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveUseCase(IToDoTaskRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public void ProcessRequest(RemoveUcRequest request)
    {
        var validationHandler = new RemoveValidationHandler(_repository);
        var persistenceHandler = new RemovePersistenceHandler(_repository, _unitOfWork);

        validationHandler.SetNext(persistenceHandler);

        validationHandler.ProcessRequest(request);
    }
}
