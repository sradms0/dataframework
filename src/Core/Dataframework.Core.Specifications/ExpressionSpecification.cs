using System.Linq.Expressions;

namespace Queueware.Dataframework.Core.Specifications;

public class ExpressionSpecification<T>(Expression<Func<T, bool>> expression) : Specification<T> where T : class
{
    private readonly Expression<Func<T, bool>> _expression;

    public override Expression<Func<T, bool>> ToExpression() => throw new NotImplementedException();
}