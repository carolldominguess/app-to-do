using App.ToDo.Application.Handlers.Add;
using App.ToDo.Application.Interfaces.UseCases;
using App.ToDo.Application.Requests;
using App.ToDo.Domain.Interfaces;
using App.ToDo.Domain.Interfaces.Repositories;

namespace App.ToDo.Application.UseCases;

public class AddUseCase : IAddUseCase
{
    private readonly IToDoTaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddUseCase(IToDoTaskRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public void ProcessRequest(AddUcRequest request)
    {
        var validationHandler = new AddValidationHandler();
        var persistenceHandler = new AddPersistenceHandler(_repository, _unitOfWork);

        validationHandler.SetNext(persistenceHandler);

        validationHandler.ProcessRequest(request);
    }
}
