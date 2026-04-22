using App.ToDo.Application.Requests;

namespace App.ToDo.Application.Handlers;

/// <summary>
/// Handler genérico base para o padrão Chain of Responsibility.
/// Cada handler concreto herda este, implementa <see cref="Handle"/> e chama o próximo da cadeia.
/// </summary>
public abstract class Handler<T> where T : Request
{
    private Handler<T>? _next;

    /// <summary>
    /// Define o próximo handler da cadeia e retorna ele para encadeamento fluente.
    /// </summary>
    public Handler<T> SetNext(Handler<T> next)
    {
        _next = next;
        return next;
    }

    /// <summary>
    /// Ponto de entrada do handler. Se o request ainda for válido, processa e passa adiante.
    /// </summary>
    public void ProcessRequest(T request)
    {
        if (!request.IsValid)
            return;

        Handle(request);

        if (request.IsValid)
            _next?.ProcessRequest(request);
    }

    /// <summary>
    /// Lógica específica do handler concreto.
    /// </summary>
    protected abstract void Handle(T request);
}
