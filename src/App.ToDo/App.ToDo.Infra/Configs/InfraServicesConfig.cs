using System.Data;
using App.ToDo.Domain.Interfaces;
using App.ToDo.Domain.Interfaces.Repositories;
using App.ToDo.Infra.Context;
using App.ToDo.Infra.Dapper;
using App.ToDo.Infra.Mappings;
using App.ToDo.Infra.Repositories;
using AutoMapper.Extensions.ExpressionMapping;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.ToDo.Infra.Configs;

public static class InfraServicesConfig
{
    public static IServiceCollection AddInfraServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

        SqlMapper.AddTypeHandler(new ToDoStatusTypeHandler());

        services.AddAutoMapper(cfg => cfg.AddExpressionMapping(), typeof(ToDoTaskProfile).Assembly);

        services.AddScoped<IUnitOfWork, App.ToDo.Infra.UnitOfWork.UnitOfWork>();
        services.AddScoped<IToDoTaskRepository, ToDoTaskRepository>();
        services.AddScoped<ILogRepository, LogRepository>();

        return services;
    }
}