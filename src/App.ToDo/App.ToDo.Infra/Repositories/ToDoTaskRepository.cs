using System.Data;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Filters;
using App.ToDo.Domain.Interfaces.Repositories;
using App.ToDo.Domain.Pagination;
using App.ToDo.Infra.Context;
using App.ToDo.Infra.Entities;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace App.ToDo.Infra.Repositories;

public class ToDoTaskRepository : Repository<ToDoTask, ToDoTaskEntity>, IToDoTaskRepository
{
    private readonly IDbConnection _dbConnection;

    public ToDoTaskRepository(AppDbContext context, IMapper mapper, IDbConnection dbConnection)
        : base(context, mapper)
    {
        _dbConnection = dbConnection;
    }

    /// <inheritdoc/>
    public PagedResult<ToDoTask> GetAllPaged(int page, int pageSize)
    {
        var offset = (page - 1) * pageSize;

        var totalItems = _dbConnection.ExecuteScalar<int>(
            "SELECT COUNT(1) FROM ToDoTasks");

        var entities = _dbConnection.Query<ToDoTaskEntity>(
            @"SELECT Id, Title, Description, Status, DueDate, CreatedAt, UpdatedAt
              FROM ToDoTasks
              ORDER BY CreatedAt DESC
              OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY",
            new { Offset = offset, PageSize = pageSize });

        var items = _mapper.Map<List<ToDoTask>>(entities.ToList());

        return new PagedResult<ToDoTask>(items, page, pageSize, totalItems);
    }

    /// <inheritdoc/>
    public PagedResult<ToDoTask> Search(ToDoTaskFilter filter, int page, int pageSize)
    {
        var domainPredicate = filter.ToPredicate();
        var entityPredicate = _mapper.MapExpression<System.Linq.Expressions.Expression<Func<ToDoTaskEntity, bool>>>(domainPredicate);

        var query = _context.ToDoTasks.AsNoTracking().Where(entityPredicate);

        var totalItems = query.Count();

        var entities = query
            .OrderBy(x => x.DueDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var items = _mapper.Map<List<ToDoTask>>(entities);

        return new PagedResult<ToDoTask>(items, page, pageSize, totalItems);
    }
}