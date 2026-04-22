using App.ToDo.Application.Interfaces.UseCases;
using App.ToDo.Application.Requests;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Interfaces.Repositories;

namespace App.ToDo.Application.UseCases;

public class GetByIdUseCase : IGetByIdUseCase
{
    private readonly IToDoTaskRepository _repository;

    public GetByIdUseCase(IToDoTaskRepository repository)
    {
        _repository = repository;
    }

    public ToDoTask? ProcessRequest(GetByIdUcRequest request)
    {
        if (request.Id == Guid.Empty)
        {
            request.AddError("Id inválido.");
            return null;
        }

        var entity = _repository.GetById(request.Id);

        if (entity is null)
            request.AddError($"Tarefa com Id '{request.Id}' não encontrada.");

        return entity;
    }
}
