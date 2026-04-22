using App.ToDo.Application.Interfaces.UseCases;
using App.ToDo.Application.Requests;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Interfaces.Repositories;
using App.ToDo.Domain.Pagination;

namespace App.ToDo.Application.UseCases;

public class GetAllUseCase : IGetAllUseCase
{
    private readonly IToDoTaskRepository _repository;

    public GetAllUseCase(IToDoTaskRepository repository)
    {
        _repository = repository;
    }

    public PagedResult<ToDoTask>? ProcessRequest(GetAllUcRequest request)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;

        return _repository.GetAllPaged(page, pageSize);
    }
}
