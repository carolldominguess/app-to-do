using App.ToDo.Application.Interfaces.UseCases;
using App.ToDo.Application.Requests;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Enums;
using App.ToDo.Domain.Filters;
using App.ToDo.Domain.Interfaces.Repositories;
using App.ToDo.Domain.Pagination;

namespace App.ToDo.Application.UseCases;

public class SearchUseCase : ISearchUseCase
{
    private readonly IToDoTaskRepository _repository;
    private readonly ILogRepository _logRepository;

    public SearchUseCase(IToDoTaskRepository repository, ILogRepository logRepository)
    {
        _repository = repository;
        _logRepository = logRepository;
    }

    public PagedResult<ToDoTask>? ProcessRequest(SearchUcRequest request)
    {
        try
        {
            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;

            var filter = new ToDoTaskFilter
            {
                Title = request.Title,
                Status = request.Status,
                DueDateFrom = request.DueDateFrom,
                DueDateTo = request.DueDateTo
            };

            if (!filter.HasAnyFilter)
            {
                var msg = "Informe ao menos um filtro para a busca.";
                request.AddError(msg);
                _logRepository.Save(new Log(nameof(SearchUseCase), LogStatus.Error, msg));
                return null;
            }

            var result = _repository.Search(filter, page, pageSize);
            _logRepository.Save(new Log(nameof(SearchUseCase), LogStatus.Success, $"Busca retornou {result.TotalItems} tarefa(s)."));
            return result;
        }
        catch (Exception ex)
        {
            _logRepository.Save(new Log(nameof(SearchUseCase), LogStatus.Error, ex.Message + " - " + ex.StackTrace));
            throw;
        }
    }
}
