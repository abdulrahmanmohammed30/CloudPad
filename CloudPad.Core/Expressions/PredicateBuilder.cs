using System.Linq.Expressions;

namespace NoteTakingApp.Core.Expressions;

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T));
        var combined = Expression.AndAlso(
            new ReplaceParameterVisitor(first.Parameters[0], parameter).Visit(first.Body),
            new ReplaceParameterVisitor(second.Parameters[0], parameter).Visit(second.Body)
        );
        return Expression.Lambda<Func<T, bool>>(combined, parameter);
    }
}

public class ReplaceParameterVisitor : ExpressionVisitor
{
    private readonly ParameterExpression _oldParameter;
    private readonly ParameterExpression _newParameter;

    public ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        _oldParameter = oldParameter;
        _newParameter = newParameter;
    }

    protected override Expression VisitParameter(ParameterExpression node)
        => node == _oldParameter ? _newParameter : base.VisitParameter(node);
}
