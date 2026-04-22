using App.ToDo.Application.Requests;

namespace App.ToDo.Application.Interfaces;

/// <summary>
/// Contrato de um UseCase que retorna um resultado.
/// </summary>
public interface IUseCaseWithResult<TRequest, TResult> where TRequest : Request
{
    TResult? ProcessRequest(TRequest request);
}
