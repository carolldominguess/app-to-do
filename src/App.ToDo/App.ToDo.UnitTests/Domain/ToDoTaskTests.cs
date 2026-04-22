using App.ToDo.Domain.Enums;
using App.ToDo.Domain.Validations;
using App.ToDo.UnitTests.Builders;
using FluentAssertions;

namespace App.ToDo.UnitTests.Domain;

public class ToDoTaskTests
{
    [Fact]
    public void ToDoTask_WhenCreatedWithValidData_ShouldBeValid()
    {
        var task = ToDoTaskBuilder.New().Build();
        var validator = new ToDoTaskValidator();

        var result = validator.Validate(task);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ToDoTask_WhenTitleIsEmpty_ShouldBeInvalid()
    {
        var task = ToDoTaskBuilder.New().WithTitle(string.Empty).Build();
        var validator = new ToDoTaskValidator();

        var result = validator.Validate(task);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "O título é obrigatório.");
    }

    [Fact]
    public void ToDoTask_WhenTitleExceedsMaxLength_ShouldBeInvalid()
    {
        var longTitle = new string('A', 151);
        var task = ToDoTaskBuilder.New().WithTitle(longTitle).Build();
        var validator = new ToDoTaskValidator();

        var result = validator.Validate(task);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "O título deve ter no máximo 150 caracteres.");
    }

    [Fact]
    public void ToDoTask_WhenDescriptionIsEmpty_ShouldBeInvalid()
    {
        var task = ToDoTaskBuilder.New().WithDescription(string.Empty).Build();
        var validator = new ToDoTaskValidator();

        var result = validator.Validate(task);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "A descrição é obrigatória.");
    }

    [Fact]
    public void ToDoTask_Update_ShouldUpdateFieldsAndSetUpdatedAt()
    {
        var task = ToDoTaskBuilder.New().Build();

        task.Update("Novo título", "Nova descrição", DateTime.UtcNow.AddDays(1), ToDoStatus.InProgress);

        task.Title.Should().Be("Novo título");
        task.Description.Should().Be("Nova descrição");
        task.Status.Should().Be(ToDoStatus.InProgress);
        task.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void ToDoTask_WhenCreated_ShouldHavePendingStatusByDefault()
    {
        var task = ToDoTaskBuilder.New().Build();

        task.Status.Should().Be(ToDoStatus.Pending);
    }

    [Fact]
    public void ToDoTask_WhenCreated_ShouldHaveCreatedAtSet()
    {
        var before = DateTime.UtcNow.AddSeconds(-1);
        var task = ToDoTaskBuilder.New().Build();
        var after = DateTime.UtcNow.AddSeconds(1);

        task.CreatedAt.Should().BeAfter(before).And.BeBefore(after);
    }
}
