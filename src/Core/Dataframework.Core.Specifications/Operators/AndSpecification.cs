using System.Linq.Expressions;
using Queueware.Dataframework.Abstractions.Specifications;

namespace Queueware.Dataframework.Core.Specifications.Operators;

public class AndSpecification<T>(ISpecification<T> leftSpecification, ISpecification<T> rightSpecification)
    : Specification<T> where T : class
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = leftSpecification.ToExpression();
        var rightExpression = rightSpecification.ToExpression();
        
        var andAlsoExpression = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
        var expressionParameter = leftExpression.Parameters[0];
        
        return Expression.Lambda<Func<T, bool>>(andAlsoExpression, expressionParameter);
    }
}