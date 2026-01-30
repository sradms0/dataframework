using System.Linq.Expressions;

namespace Queueware.Dataframework.Core.Specifications.Internal;

internal static class ExpressionComposer
{
    public static Expression<Func<T, bool>> AndAlso<T>(Expression<Func<T, bool>> leftExpression,
        Expression<Func<T, bool>> rightExpression)
    {
        var fromParameterExpression = rightExpression.Parameters[0];
        var toParameterExpression = leftExpression.Parameters[0];
        var updatedRightExpressionBody = new ParameterVisitor(fromParameterExpression, toParameterExpression)
            .Visit(rightExpression.Body);
        
        var andAlsoExpressionBody = Expression.AndAlso(leftExpression.Body, updatedRightExpressionBody);
        return Expression.Lambda<Func<T, bool>>(andAlsoExpressionBody, toParameterExpression);
    }

    private sealed class ParameterVisitor(ParameterExpression fromParameterExpression, 
        ParameterExpression toParameterExpression) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression currentParameterExpression)
        {
            return ReferenceEquals(currentParameterExpression, fromParameterExpression)
                ? toParameterExpression
                : base.VisitParameter(currentParameterExpression);
        }
    }
}