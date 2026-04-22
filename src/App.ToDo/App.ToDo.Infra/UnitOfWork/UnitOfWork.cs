using App.ToDo.Domain.Interfaces;
using App.ToDo.Infra.Context;

namespace App.ToDo.Infra.UnitOfWork;

/// <summary>
/// Implementação do padrão Unit of Work.
/// Garante que todas as operações do ciclo sejam persistidas em uma única transação.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private bool _disposed;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public int Commit() => _context.SaveChanges();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
            _context.Dispose();

        _disposed = true;
    }
}
