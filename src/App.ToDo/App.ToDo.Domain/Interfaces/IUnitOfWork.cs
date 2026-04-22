namespace App.ToDo.Domain.Interfaces;
public interface IUnitOfWork : IDisposable
{
    int Commit();
}