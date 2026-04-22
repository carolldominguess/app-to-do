using App.ToDo.Application.Handlers.Add;
using App.ToDo.Application.Interfaces.UseCases;
using App.ToDo.Application.Requests;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Enums;
using App.ToDo.Domain.Interfaces;
using App.ToDo.Domain.Interfaces.Repositories;

namespace App.ToDo.Application.UseCases;

public class AddUseCase : IAddUseCase
{
    private readonly IToDoTaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogRepository _logRepository;

    public AddUseCase(IToDoTaskRepository repository, IUnitOfWork unitOfWork, ILogRepository logRepository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logRepository = logRepository;
    }

    public void ProcessRequest(AddUcRequest request)
    {
        try
        {
            var validationHandler = new AddValidationHandler();
            var persistenceHandler = new AddPersistenceHandler(_repository, _unitOfWork);
            validationHandler.SetNext(persistenceHandler);
            validationHandler.ProcessRequest(request);

            var status = request.IsValid ? LogStatus.Success : LogStatus.Error;
            var message = request.IsValid
                ? "Tarefa criada com sucesso."
                : string.Join("; ", request.Errors);

            _logRepository.Save(new Log(nameof(AddUseCase), status, message));
        }
        catch (Exception ex)
        {
            _logRepository.Save(new Log(nameof(AddUseCase), LogStatus.Error, ex.Message + " - " + ex.StackTrace));
            throw;
        }
    }
}