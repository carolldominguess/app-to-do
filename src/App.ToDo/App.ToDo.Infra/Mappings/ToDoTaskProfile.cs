using App.ToDo.Domain.Entities;
using App.ToDo.Infra.Entities;
using AutoMapper;

namespace App.ToDo.Infra.Mappings;

public class ToDoTaskProfile : Profile
{
    public ToDoTaskProfile()
    {
        // Domain → Entity (para persistência)
        CreateMap<ToDoTask, ToDoTaskEntity>();

        // Entity → Domain (para retorno ao domínio)
        // Usa o construtor sem parâmetros (protected) via ForMember para contornar o encapsulamento
        CreateMap<ToDoTaskEntity, ToDoTask>()
            .ConstructUsing(_ => CreateEmptyDomain())
            .ForMember(dest => dest.Id,          opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title,       opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Status,      opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DueDate,     opt => opt.MapFrom(src => src.DueDate))
            .ForMember(dest => dest.CreatedAt,   opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt,   opt => opt.MapFrom(src => src.UpdatedAt));
    }

    /// <summary>
    /// Cria uma instância vazia de <see cref="ToDoTask"/> via reflection,
    /// contornando o construtor protegido sem violar o encapsulamento do domínio externamente.
    /// </summary>
    private static ToDoTask CreateEmptyDomain() =>
        (ToDoTask)System.Runtime.CompilerServices.RuntimeHelpers
            .GetUninitializedObject(typeof(ToDoTask));
}