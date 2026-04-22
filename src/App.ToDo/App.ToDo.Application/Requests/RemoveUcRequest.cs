namespace App.ToDo.Application.Requests;

public class RemoveUcRequest : Request
{
    public Guid Id { get; init; }
}
