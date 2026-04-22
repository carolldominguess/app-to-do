using System.Linq.Expressions;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Enums;
using App.ToDo.Domain.Helpers;

namespace App.ToDo.Domain.Filters;
public class ToDoTaskFilter
{
    public ToDoStatus? Status { get; set; }
    public DateTime? DueDateFrom { get; set; }
    public DateTime? DueDateTo { get; set; }
    public string? Title { get; set; }

    public bool HasAnyFilter =>
        Status.HasValue ||
        DueDateFrom.HasValue ||
        DueDateTo.HasValue ||
        !string.IsNullOrWhiteSpace(Title);

    public Expression<Func<ToDoTask, bool>> ToPredicate()
    {
        var predicate = PredicateBuilder.True<ToDoTask>();

        if (Status.HasValue)
            predicate = predicate.And(x => x.Status == Status.Value);

        if (DueDateFrom.HasValue)
            predicate = predicate.And(x => x.DueDate >= DueDateFrom.Value);

        if (DueDateTo.HasValue)
            predicate = predicate.And(x => x.DueDate <= DueDateTo.Value);

        if (!string.IsNullOrWhiteSpace(Title))
        {
            var title = Title.Trim();
            predicate = predicate.And(x => x.Title.Contains(title));
        }

        return predicate;
    }
}