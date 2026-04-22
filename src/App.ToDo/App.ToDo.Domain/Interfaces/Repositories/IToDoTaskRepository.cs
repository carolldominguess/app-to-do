using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Filters;
using App.ToDo.Domain.Pagination;

namespace App.ToDo.Domain.Interfaces.Repositories;

public interface IToDoTaskRepository : IRepository<ToDoTask>
{
    PagedResult<ToDoTask> GetAllPaged(int page, int pageSize);
    PagedResult<ToDoTask> Search(ToDoTaskFilter filter, int page, int pageSize);
}