using System.Linq.Expressions;

namespace App.ToDo.Domain.Helpers;

/// <summary>
/// Construtor de predicados para expressões dinâmicas.
/// Permite compor filtros com AND/OR preservando a árvore de expressão original.
/// </summary>
public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> True<T>() => _ => true;
    public static Expression<Func<T, bool>> False<T>() => _ => false;

    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> a,
        Expression<Func<T, bool>> b) => Combine(a, b, Expression.AndAlso);

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> a,
        Expression<Func<T, bool>> b) => Combine(a, b, Expression.OrElse);

    private static Expression<Func<T, bool>> Combine<T>(
        Expression<Func<T, bool>> a,
        Expression<Func<T, bool>> b,
        Func<Expression, Expression, BinaryExpression> merge)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        var leftVisitor = new ReplaceParameterVisitor(a.Parameters[0], parameter);
        var left = leftVisitor.Visit(a.Body)!;

        var rightVisitor = new ReplaceParameterVisitor(b.Parameters[0], parameter);
        var right = rightVisitor.Visit(b.Body)!;

        return Expression.Lambda<Func<T, bool>>(merge(left, right), parameter);
    }

    private sealed class ReplaceParameterVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _source;
        private readonly ParameterExpression _target;

        public ReplaceParameterVisitor(ParameterExpression source, ParameterExpression target)
        {
            _source = source;
            _target = target;
        }

        protected override Expression VisitParameter(ParameterExpression node)
            => node == _source ? _target : base.VisitParameter(node);
    }
}