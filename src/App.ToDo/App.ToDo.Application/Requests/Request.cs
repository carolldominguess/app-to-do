namespace App.ToDo.Application.Requests;

/// <summary>
/// Classe base para todos os requests da camada Application.
/// </summary>
public abstract class Request
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public bool IsValid { get; set; } = true;
    public IList<string> Errors { get; set; } = new List<string>();

    public void AddError(string error)
    {
        IsValid = false;
        Errors.Add(error);
    }
}
