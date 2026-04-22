using App.ToDo.Domain.Entities;
using App.ToDo.Infra.Entities;
using AutoMapper;

namespace App.ToDo.Infra.Mappings;

public class LogProfile : Profile
{
    public LogProfile()
    {
        CreateMap<Log, LogEntity>();

        CreateMap<LogEntity, Log>()
            .ConstructUsing(_ => (Log)System.Runtime.CompilerServices.RuntimeHelpers
                .GetUninitializedObject(typeof(Log)));
    }
}
