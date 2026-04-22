using App.ToDo.Application.Handlers.Remove;
using App.ToDo.Application.Interfaces.UseCases;
using App.ToDo.Application.Requests;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Enums;
using App.ToDo.Domain.Interfaces;
using App.ToDo.Domain.Interfaces.Repositories;

namespace App.ToDo.Application.UseCases;

public class RemoveUseCase : IRemoveUseCase
{
    private readonly IToDoTaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogRepository _logRepository;

    public RemoveUseCase(IToDoTaskRepository repository, IUnitOfWork unitOfWork, ILogRepository logRepository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logRepository = logRepository;
    }

    public void ProcessRequest(RemoveUcRequest request)
    {
        try
        {
            var validationHandler = new RemoveValidationHandler(_repository);
            var persistenceHandler = new RemovePersistenceHandler(_repository, _unitOfWork);
            validationHandler.SetNext(persistenceHandler);
            validationHandler.ProcessRequest(request);

            var status = request.IsValid ? LogStatus.Success : LogStatus.Error;
            var message = request.IsValid
                ? "Tarefa removida com sucesso."
                : string.Join("; ", request.Errors);

            _logRepository.Save(new Log(nameof(RemoveUseCase), status, message));
        }
        catch (Exception ex)
        {
            _logRepository.Save(new Log(nameof(RemoveUseCase), LogStatus.Error, ex.Message + " - " + ex.StackTrace));
            throw;
        }
    }
}
