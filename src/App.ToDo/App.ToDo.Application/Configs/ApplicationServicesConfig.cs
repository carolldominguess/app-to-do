using App.ToDo.Application.Interfaces.UseCases;
using App.ToDo.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace App.ToDo.Application.Configs;

public static class ApplicationServicesConfig
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAddUseCase, AddUseCase>();
        services.AddScoped<IUpdateUseCase, UpdateUseCase>();
        services.AddScoped<IRemoveUseCase, RemoveUseCase>();
        services.AddScoped<IGetByIdUseCase, GetByIdUseCase>();
        services.AddScoped<IGetAllUseCase, GetAllUseCase>();
        services.AddScoped<ISearchUseCase, SearchUseCase>();

        return services;
    }
}
