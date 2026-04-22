using App.ToDo.Application.Interfaces.UseCases;
using App.ToDo.Application.Requests;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Enums;
using App.ToDo.Domain.Interfaces.Repositories;
using App.ToDo.Domain.Pagination;

namespace App.ToDo.Application.UseCases;

public class GetAllUseCase : IGetAllUseCase
{
    private readonly IToDoTaskRepository _repository;
    private readonly ILogRepository _logRepository;

    public GetAllUseCase(IToDoTaskRepository repository, ILogRepository logRepository)
    {
        _repository = repository;
        _logRepository = logRepository;
    }

    public PagedResult<ToDoTask>? ProcessRequest(GetAllUcRequest request)
    {
        try
        {
            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;

            var result = _repository.GetAllPaged(page, pageSize);
            _logRepository.Save(new Log(nameof(GetAllUseCase), LogStatus.Success, $"Consulta retornou {result.TotalItems} tarefa(s)."));
            return result;
        }
        catch (Exception ex)
        {
            _logRepository.Save(new Log(nameof(GetAllUseCase), LogStatus.Error, ex.Message + " - " + ex.StackTrace));
            throw;
        }
    }
}
