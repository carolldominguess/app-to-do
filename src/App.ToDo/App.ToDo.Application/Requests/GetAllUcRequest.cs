namespace App.ToDo.Application.Requests;

public class GetAllUcRequest : Request
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
