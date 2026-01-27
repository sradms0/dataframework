using System.Linq.Expressions;
using Queueware.Dataframework.Abstractions.Specifications;

namespace Queueware.Dataframework.Core.Specifications;

public abstract class Specification<T> : ISpecification<T> where T : class
{
    public bool IsSatisfiedBy(T candidate) => throw new NotImplementedException();

    public abstract Expression<Func<T, bool>> ToExpression();
}