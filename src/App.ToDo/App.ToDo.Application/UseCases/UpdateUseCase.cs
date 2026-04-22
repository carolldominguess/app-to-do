using App.ToDo.Application.Handlers.Update;
using App.ToDo.Application.Interfaces.UseCases;
using App.ToDo.Application.Requests;
using App.ToDo.Domain.Interfaces;
using App.ToDo.Domain.Interfaces.Repositories;

namespace App.ToDo.Application.UseCases;

public class UpdateUseCase : IUpdateUseCase
{
    private readonly IToDoTaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUseCase(IToDoTaskRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public void ProcessRequest(UpdateUcRequest request)
    {
        var validationHandler = new UpdateValidationHandler();
        var persistenceHandler = new UpdatePersistenceHandler(_repository, _unitOfWork);

        validationHandler.SetNext(persistenceHandler);

        validationHandler.ProcessRequest(request);
    }
}
