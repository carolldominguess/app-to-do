using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Interfaces.Repositories;
using App.ToDo.Infra.Context;
using App.ToDo.Infra.Entities;
using AutoMapper;

namespace App.ToDo.Infra.Repositories;

/// <summary>
/// Repositório de logs com persistência independente do UnitOfWork,
/// garantindo que erros sejam registrados mesmo quando a operação principal falha.
/// </summary>
public class LogRepository : ILogRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public LogRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void Save(Log log)
    {
        var entity = _mapper.Map<LogEntity>(log);
        _context.Logs.Add(entity);
        _context.SaveChanges();
    }
}