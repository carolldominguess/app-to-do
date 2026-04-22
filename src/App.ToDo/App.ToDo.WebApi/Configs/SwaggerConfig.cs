using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace App.ToDo.WebApi.Configs;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "App.ToDo API",
                Version = "v1",
                Description = "API RESTful para gestão de tarefas (ToDo) com Clean Architecture.",
                Contact = new OpenApiContact
                {
                    Name = "App.ToDo",
                    Email = "contato@apptodo.dev"
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                c.IncludeXmlComments(xmlPath);

            c.EnableAnnotations();
            c.UseInlineDefinitionsForEnums();
            c.SchemaFilter<EnumSchemaFilter>();
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "App.ToDo API v1");
            c.RoutePrefix = "swagger";
        });

        return app;
    }
}

/// <summary>
/// Substitui o schema de enums no Swagger para exibir os nomes string
/// em vez de valores numéricos, alinhado com o JsonStringEnumConverter.
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum)
            return;

        schema.Type = "string";
        schema.Format = null;
        schema.Enum = Enum.GetNames(context.Type)
            .Select(name => (Microsoft.OpenApi.Any.IOpenApiAny)new Microsoft.OpenApi.Any.OpenApiString(name))
            .ToList();
    }
}