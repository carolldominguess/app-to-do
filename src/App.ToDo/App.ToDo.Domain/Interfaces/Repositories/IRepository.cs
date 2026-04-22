using System.Linq.Expressions;

namespace App.ToDo.Domain.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    T? GetById(Guid id);
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
}