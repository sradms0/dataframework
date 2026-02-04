using System.Linq.Expressions;
using Queueware.Dataframework.Abstractions.Specifications;

namespace Queueware.Dataframework.Core.Specifications.Operators;

public class NotSpecification<T>(ISpecification<T> specification) : Specification<T> where T : class
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        throw new NotImplementedException();
    }
}