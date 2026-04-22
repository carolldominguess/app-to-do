namespace App.ToDo.WebApi.Responses;

public class ErrorResponse
{
    public IList<string> Errors { get; init; } = new List<string>();

    public ErrorResponse() { }

    public ErrorResponse(IEnumerable<string> errors)
    {
        Errors = errors.ToList();
    }

    public ErrorResponse(string error)
    {
        Errors = new List<string> { error };
    }
}
