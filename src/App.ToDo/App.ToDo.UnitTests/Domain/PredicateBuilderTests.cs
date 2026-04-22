using App.ToDo.Domain.Enums;
using App.ToDo.Domain.Filters;
using App.ToDo.UnitTests.Builders;
using FluentAssertions;

namespace App.ToDo.UnitTests.Domain;

public class PredicateBuilderTests
{
    [Fact]
    public void ToPredicate_WithStatusFilter_ShouldFilterCorrectly()
    {
        var tasks = new[]
        {
            ToDoTaskBuilder.New().WithStatus(ToDoStatus.Pending).Build(),
            ToDoTaskBuilder.New().WithStatus(ToDoStatus.InProgress).Build(),
            ToDoTaskBuilder.New().WithStatus(ToDoStatus.Completed).Build()
        };

        var filter = new ToDoTaskFilter { Status = ToDoStatus.Pending };
        var predicate = filter.ToPredicate().Compile();

        var result = tasks.Where(predicate).ToList();

        result.Should().HaveCount(1);
        result[0].Status.Should().Be(ToDoStatus.Pending);
    }

    [Fact]
    public void ToPredicate_WithDueDateFromFilter_ShouldFilterCorrectly()
    {
        var reference = DateTime.UtcNow;

        var tasks = new[]
        {
            ToDoTaskBuilder.New().WithDueDate(reference.AddDays(-1)).Build(),
            ToDoTaskBuilder.New().WithDueDate(reference.AddDays(1)).Build(),
            ToDoTaskBuilder.New().WithDueDate(reference.AddDays(5)).Build()
        };

        var filter = new ToDoTaskFilter { DueDateFrom = reference };
        var predicate = filter.ToPredicate().Compile();

        var result = tasks.Where(predicate).ToList();

        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.DueDate >= reference);
    }

    [Fact]
    public void ToPredicate_WithTitleFilter_ShouldFilterCorrectly()
    {
        var tasks = new[]
        {
            ToDoTaskBuilder.New().WithTitle("Comprar mantimentos").Build(),
            ToDoTaskBuilder.New().WithTitle("Reunião de equipe").Build(),
            ToDoTaskBuilder.New().WithTitle("Comprar presente").Build()
        };

        var filter = new ToDoTaskFilter { Title = "Comprar" };
        var predicate = filter.ToPredicate().Compile();

        var result = tasks.Where(predicate).ToList();

        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.Title.Contains("Comprar"));
    }

    [Fact]
    public void ToPredicate_WithNoFilters_ShouldReturnAll()
    {
        var tasks = new[]
        {
            ToDoTaskBuilder.New().Build(),
            ToDoTaskBuilder.New().Build(),
            ToDoTaskBuilder.New().Build()
        };

        var filter = new ToDoTaskFilter();
        var predicate = filter.ToPredicate().Compile();

        var result = tasks.Where(predicate).ToList();

        result.Should().HaveCount(3);
    }

    [Fact]
    public void ToPredicate_WithCombinedFilters_ShouldApplyAndLogic()
    {
        var reference = DateTime.UtcNow;

        var tasks = new[]
        {
            ToDoTaskBuilder.New().WithStatus(ToDoStatus.Pending).WithDueDate(reference.AddDays(1)).Build(),
            ToDoTaskBuilder.New().WithStatus(ToDoStatus.InProgress).WithDueDate(reference.AddDays(1)).Build(),
            ToDoTaskBuilder.New().WithStatus(ToDoStatus.Pending).WithDueDate(reference.AddDays(-1)).Build()
        };

        var filter = new ToDoTaskFilter
        {
            Status = ToDoStatus.Pending,
            DueDateFrom = reference
        };

        var predicate = filter.ToPredicate().Compile();

        var result = tasks.Where(predicate).ToList();

        result.Should().HaveCount(1);
        result[0].Status.Should().Be(ToDoStatus.Pending);
        result[0].DueDate.Should().BeAfter(reference.AddSeconds(-1));
    }
}
