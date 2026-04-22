using App.ToDo.Application.Interfaces.UseCases;
using App.ToDo.Application.Requests;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Filters;
using App.ToDo.Domain.Interfaces.Repositories;
using App.ToDo.Domain.Pagination;

namespace App.ToDo.Application.UseCases;

public class SearchUseCase : ISearchUseCase
{
    private readonly IToDoTaskRepository _repository;

    public SearchUseCase(IToDoTaskRepository repository)
    {
        _repository = repository;
    }

    public PagedResult<ToDoTask>? ProcessRequest(SearchUcRequest request)
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
            request.AddError("Informe ao menos um filtro para a busca.");
            return null;
        }

        return _repository.Search(filter, page, pageSize);
    }
}
