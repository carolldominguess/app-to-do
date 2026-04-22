using App.ToDo.Application.Requests;
using App.ToDo.Domain.Entities;

namespace App.ToDo.Application.Interfaces.UseCases;

public interface IGetByIdUseCase : IUseCaseWithResult<GetByIdUcRequest, ToDoTask> { }
