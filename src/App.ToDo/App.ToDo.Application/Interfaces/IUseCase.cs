using App.ToDo.Application.Requests;

namespace App.ToDo.Application.Interfaces;

/// <summary>
/// Contrato base de um UseCase. Define o ponto de entrada com o método ProcessRequest.
/// </summary>
public interface IUseCase<TRequest> where TRequest : Request
{
    void ProcessRequest(TRequest request);
}
