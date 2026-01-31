using System.Linq.Expressions;

namespace Queueware.Dataframework.Core.Specifications.Internal;

internal static class ExpressionComposer
{
    public static Expression<Func<T, bool>> AndAlso<T>(Expression<Func<T, bool>> leftExpression,
        Expression<Func<T, bool>> rightExpression)
    {
        var syncedRightExpressionBody = SyncRightExpressionParametersToLeft(leftExpression, rightExpression);
        var andAlsoExpressionBody = Expression.AndAlso(leftExpression.Body, syncedRightExpressionBody);
        
        return Expression.Lambda<Func<T, bool>>(andAlsoExpressionBody, leftExpression.Parameters[0]);
    }

    public static Expression<Func<T, bool>> AndAlsoNot<T>(Expression<Func<T, bool>> leftExpression,
        Expression<Func<T, bool>> rightExpression)
    {
        var syncedRightExpressionBody = SyncRightExpressionParametersToLeft(leftExpression, rightExpression);
        syncedRightExpressionBody = Expression.Not(syncedRightExpressionBody);
        var andAlsoExpressionBody = Expression.AndAlso(leftExpression.Body, syncedRightExpressionBody);
        
        return Expression.Lambda<Func<T, bool>>(andAlsoExpressionBody, leftExpression.Parameters[0]);
    }

    public static Expression<Func<T, bool>> OrElse<T>(Expression<Func<T, bool>> leftExpression,
        Expression<Func<T, bool>> rightExpression)
    {
        var syncedRightExpressionBody = SyncRightExpressionParametersToLeft(leftExpression, rightExpression);
        var orExpressionBody = Expression.OrElse(leftExpression.Body, syncedRightExpressionBody);
        
        return Expression.Lambda<Func<T, bool>>(orExpressionBody, leftExpression.Parameters[0]);
    }
    
    public static Expression<Func<T, bool>> OrElseNot<T>(Expression<Func<T, bool>> leftExpression,
        Expression<Func<T, bool>> rightExpression)
    {
        var syncedRightExpressionBody = SyncRightExpressionParametersToLeft(leftExpression, rightExpression);
        syncedRightExpressionBody = Expression.Not(syncedRightExpressionBody);
        var orNotExpressionBody = Expression.OrElse(leftExpression.Body, syncedRightExpressionBody);
        
        return Expression.Lambda<Func<T, bool>>(orNotExpressionBody, leftExpression.Parameters[0]);
    }

    private static Expression SyncRightExpressionParametersToLeft<T>(Expression<Func<T, bool>> leftExpression,
        Expression<Func<T, bool>> rightExpression)
    {
        var fromParameterExpression = rightExpression.Parameters[0];
        var toParameterExpression = leftExpression.Parameters[0];
        
        return new ParameterVisitor(fromParameterExpression, toParameterExpression)
            .Visit(rightExpression.Body);
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