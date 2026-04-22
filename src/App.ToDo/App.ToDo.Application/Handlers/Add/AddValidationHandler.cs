using App.ToDo.Application.Requests;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Validations;

namespace App.ToDo.Application.Handlers.Add;

public class AddValidationHandler : Handler<AddUcRequest>
{
    protected override void Handle(AddUcRequest request)
    {
        var entity = new ToDoTask(request.Title, request.Description, request.DueDate, request.Status);
        var validator = new ToDoTaskValidator();
        var result = validator.Validate(entity);

        if (!result.IsValid)
            foreach (var error in result.Errors)
                request.AddError(error.ErrorMessage);
    }
}
