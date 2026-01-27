using System.Linq.Expressions;

namespace Queueware.Dataframework.Core.Specifications;

public class ExpressionSpecification<T> : Specification<T> where T : class
{
    private readonly Expression<Func<T, bool>> _expression;

    public ExpressionSpecification(Expression<Func<T, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        _expression = expression;
    }

    public override Expression<Func<T, bool>> ToExpression() => _expression;
}