using System.Linq.Expressions;
using Queueware.Dataframework.Abstractions.Specifications;
using Queueware.Dataframework.Core.Specifications.Internal;

namespace Queueware.Dataframework.Core.Specifications.Operators;

public class OrNotSpecification<T>(ISpecification<T> leftSpecification, ISpecification<T> rightSpecification)
    : Specification<T> where T : class
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = leftSpecification.ToExpression();
        var rightExpression = rightSpecification.ToExpression();
        
        return ExpressionComposer.OrElseNot(leftExpression, rightExpression);
    }
}