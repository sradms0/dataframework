using System.Linq.Expressions;

namespace Queueware.Dataframework.Abstractions.Specifications;

public interface ISpecification<T> where T : class
{
    bool IsSatisfiedBy(T candidate);

    Expression<Func<T, bool>> ToExpression();
}