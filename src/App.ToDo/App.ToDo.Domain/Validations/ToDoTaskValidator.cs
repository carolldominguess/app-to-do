using App.ToDo.Domain.Entities;
using FluentValidation;

namespace App.ToDo.Domain.Validations;

public class ToDoTaskValidator : AbstractValidator<ToDoTask>
{
    public ToDoTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O título é obrigatório.")
            .MaximumLength(150).WithMessage("O título deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(1000).WithMessage("A descrição deve ter no máximo 1000 caracteres.");

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("A data de vencimento é obrigatória.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status inválido.");
    }
}