using System.Linq.Expressions;
using App.ToDo.Domain.Interfaces.Repositories;
using App.ToDo.Infra.Context;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;

namespace App.ToDo.Infra.Repositories;

/// <summary>
/// Repositório base duplo-genérico:
/// <typeparamref name="TDomain"/> = entidade do domínio (retornada ao caller)
/// <typeparamref name="TEntity"/> = POCO de persistência (usado pelo EF Core)
/// O AutoMapper faz a conversão entre os dois em todas as operações.
/// </summary>
public abstract class Repository<TDomain, TEntity> : IRepository<TDomain>
    where TDomain : class
    where TEntity : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly IMapper _mapper;

    protected Repository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
        _mapper = mapper;
    }

    public void Add(TDomain domain)
    {
        var entity = _mapper.Map<TEntity>(domain);
        _dbSet.Add(entity);
    }

    public void Update(TDomain domain)
    {
        var entity = _mapper.Map<TEntity>(domain);
        _dbSet.Update(entity);
    }

    public void Remove(TDomain domain)
    {
        var entity = _mapper.Map<TEntity>(domain);
        _dbSet.Remove(entity);
    }

    public TDomain? GetById(Guid id)
    {
        var entity = _dbSet.Find(id);
        return entity is null ? null : _mapper.Map<TDomain>(entity);
    }

    public IEnumerable<TDomain> Find(Expression<Func<TDomain, bool>> predicate)
    {
        var entityPredicate = _mapper.MapExpression<Expression<Func<TEntity, bool>>>(predicate);

        return _dbSet
            .AsNoTracking()
            .Where(entityPredicate)
            .AsEnumerable()
            .Select(e => _mapper.Map<TDomain>(e));
    }
}