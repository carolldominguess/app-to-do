using App.ToDo.Application.Requests;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Pagination;

namespace App.ToDo.Application.Interfaces.UseCases;

public interface ISearchUseCase : IUseCaseWithResult<SearchUcRequest, PagedResult<ToDoTask>> { }
