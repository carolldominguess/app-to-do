using App.ToDo.Application.Handlers.Update;
using App.ToDo.Application.Interfaces.UseCases;
using App.ToDo.Application.Requests;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Enums;
using App.ToDo.Domain.Interfaces;
using App.ToDo.Domain.Interfaces.Repositories;

namespace App.ToDo.Application.UseCases;

public class UpdateUseCase : IUpdateUseCase
{
    private readonly IToDoTaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogRepository _logRepository;

    public UpdateUseCase(IToDoTaskRepository repository, IUnitOfWork unitOfWork, ILogRepository logRepository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logRepository = logRepository;
    }

    public void ProcessRequest(UpdateUcRequest request)
    {
        try
        {
            var validationHandler = new UpdateValidationHandler();
            var persistenceHandler = new UpdatePersistenceHandler(_repository, _unitOfWork);
            validationHandler.SetNext(persistenceHandler);
            validationHandler.ProcessRequest(request);

            var status = request.IsValid ? LogStatus.Success : LogStatus.Error;
            var message = request.IsValid
                ? "Tarefa atualizada com sucesso."
                : string.Join("; ", request.Errors);

            _logRepository.Save(new Log(nameof(UpdateUseCase), status, message));
        }
        catch (Exception ex)
        {
            _logRepository.Save(new Log(nameof(UpdateUseCase), LogStatus.Error, ex.Message + " - " + ex.StackTrace));
            throw;
        }
    }
}
