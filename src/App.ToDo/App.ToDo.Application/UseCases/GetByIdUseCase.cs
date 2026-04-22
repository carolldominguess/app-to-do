using App.ToDo.Application.Interfaces.UseCases;
using App.ToDo.Application.Requests;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Enums;
using App.ToDo.Domain.Interfaces.Repositories;

namespace App.ToDo.Application.UseCases;

public class GetByIdUseCase : IGetByIdUseCase
{
    private readonly IToDoTaskRepository _repository;
    private readonly ILogRepository _logRepository;

    public GetByIdUseCase(IToDoTaskRepository repository, ILogRepository logRepository)
    {
        _repository = repository;
        _logRepository = logRepository;
    }

    public ToDoTask? ProcessRequest(GetByIdUcRequest request)
    {
        try
        {
            if (request.Id == Guid.Empty)
            {
                request.AddError("Id inválido.");
                _logRepository.Save(new Log(nameof(GetByIdUseCase), LogStatus.Error, "Id inválido."));
                return null;
            }

            var entity = _repository.GetById(request.Id);

            if (entity is null)
            {
                var msg = $"Tarefa com Id '{request.Id}' não encontrada.";
                request.AddError(msg);
                _logRepository.Save(new Log(nameof(GetByIdUseCase), LogStatus.Error, msg));
                return null;
            }

            _logRepository.Save(new Log(nameof(GetByIdUseCase), LogStatus.Success, "Tarefa encontrada com sucesso."));
            return entity;
        }
        catch (Exception ex)
        {
            _logRepository.Save(new Log(nameof(GetByIdUseCase), LogStatus.Error, ex.Message + " - " + ex.StackTrace));
            throw;
        }
    }
}
