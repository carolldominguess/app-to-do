using App.ToDo.Domain.Entities;

namespace App.ToDo.Domain.Interfaces.Repositories;

public interface ILogRepository
{
    void Save(Log log);
}
