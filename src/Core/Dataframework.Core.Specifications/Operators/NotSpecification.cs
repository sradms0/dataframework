using System.Linq.Expressions;
using Queueware.Dataframework.Abstractions.Specifications;

namespace Queueware.Dataframework.Core.Specifications.Operators;

public class NotSpecification<T>(ISpecification<T> specification) : Specification<T> where T : class
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var expression = specification.ToExpression();
        var expressionBody = Expression.Not(expression.Body);
        
        return Expression.Lambda<Func<T, bool>>(expressionBody, expression.Parameters[0]);
    }
}