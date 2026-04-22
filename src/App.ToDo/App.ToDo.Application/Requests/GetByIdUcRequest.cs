namespace App.ToDo.Application.Requests;

public class GetByIdUcRequest : Request
{
    public Guid Id { get; init; }
}
