using App.ToDo.Application.Interfaces.UseCases;
using App.ToDo.Application.Requests;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Pagination;
using App.ToDo.WebApi.Requests;
using App.ToDo.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace App.ToDo.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ToDoTaskController : ControllerBase
{
    private readonly IAddUseCase _addUseCase;
    private readonly IUpdateUseCase _updateUseCase;
    private readonly IRemoveUseCase _removeUseCase;
    private readonly IGetByIdUseCase _getByIdUseCase;
    private readonly IGetAllUseCase _getAllUseCase;
    private readonly ISearchUseCase _searchUseCase;

    public ToDoTaskController(
        IAddUseCase addUseCase,
        IUpdateUseCase updateUseCase,
        IRemoveUseCase removeUseCase,
        IGetByIdUseCase getByIdUseCase,
        IGetAllUseCase getAllUseCase,
        ISearchUseCase searchUseCase)
    {
        _addUseCase = addUseCase;
        _updateUseCase = updateUseCase;
        _removeUseCase = removeUseCase;
        _getByIdUseCase = getByIdUseCase;
        _getAllUseCase = getAllUseCase;
        _searchUseCase = searchUseCase;
    }

    /// <summary>Lista todas as tarefas com paginação.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<ToDoTaskResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var ucRequest = new GetAllUcRequest { Page = page, PageSize = pageSize };
        var result = _getAllUseCase.ProcessRequest(ucRequest);
        return Ok(MapPagedResult(result));
    }

    /// <summary>Busca uma tarefa pelo Id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ToDoTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult GetById(Guid id)
    {
        var ucRequest = new GetByIdUcRequest { Id = id };
        var entity = _getByIdUseCase.ProcessRequest(ucRequest);

        if (!ucRequest.IsValid)
            return BadRequest(new ErrorResponse(ucRequest.Errors));

        if (entity is null)
            return NotFound(new ErrorResponse($"Tarefa '{id}' não encontrada."));

        return Ok(MapToResponse(entity));
    }

    /// <summary>Busca tarefas com filtros (status, data de vencimento, título) com paginação.</summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResponse<ToDoTaskResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult Search([FromQuery] SearchToDoTaskRequest request)
    {
        var ucRequest = new SearchUcRequest
        {
            Title = request.Title,
            Status = request.Status,
            DueDateFrom = request.DueDateFrom,
            DueDateTo = request.DueDateTo,
            Page = request.Page,
            PageSize = request.PageSize
        };

        var result = _searchUseCase.ProcessRequest(ucRequest);

        if (!ucRequest.IsValid)
            return BadRequest(new ErrorResponse(ucRequest.Errors));

        return Ok(MapPagedResult(result));
    }

    /// <summary>Cria uma nova tarefa.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateToDoTaskRequest request)
    {
        var ucRequest = new AddUcRequest
        {
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            DueDate = request.DueDate
        };

        _addUseCase.ProcessRequest(ucRequest);

        if (!ucRequest.IsValid)
            return BadRequest(new ErrorResponse(ucRequest.Errors));

        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>Atualiza uma tarefa existente.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] UpdateToDoTaskRequest request)
    {
        var ucRequest = new UpdateUcRequest
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            DueDate = request.DueDate
        };

        _updateUseCase.ProcessRequest(ucRequest);

        if (!ucRequest.IsValid)
        {
            if (ucRequest.Errors.Any(e => e.Contains("não encontrada")))
                return NotFound(new ErrorResponse(ucRequest.Errors));

            return BadRequest(new ErrorResponse(ucRequest.Errors));
        }

        return NoContent();
    }

    /// <summary>Remove uma tarefa pelo Id.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var ucRequest = new RemoveUcRequest { Id = id };

        _removeUseCase.ProcessRequest(ucRequest);

        if (!ucRequest.IsValid)
            return NotFound(new ErrorResponse(ucRequest.Errors));

        return NoContent();
    }

    private static ToDoTaskResponse MapToResponse(ToDoTask entity) => new()
    {
        Id = entity.Id,
        Title = entity.Title,
        Description = entity.Description,
        Status = entity.Status,
        DueDate = entity.DueDate,
        CreatedAt = entity.CreatedAt,
        UpdatedAt = entity.UpdatedAt
    };

    private static PagedResponse<ToDoTaskResponse> MapPagedResult(PagedResult<ToDoTask>? result)
    {
        if (result is null)
            return new PagedResponse<ToDoTaskResponse>();

        return new PagedResponse<ToDoTaskResponse>
        {
            Items = result.Items.Select(MapToResponse).ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalItems = result.TotalItems,
            TotalPages = result.TotalPages
        };
    }
}
