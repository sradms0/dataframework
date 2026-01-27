using System.Linq.Expressions;
using Queueware.Dataframework.Core.Specifications;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications;

public class TestSpecification : Specification<MockDataType1>
{
    private Expression<Func<MockDataType1, bool>> _testExpression { get; set; } = _ => true;

    public bool isToExpressionThrowing { get; set; }
    
    public bool isTestExpressionThrowing { get; set; }

    public Expression<Func<MockDataType1, bool>> TestExpression
    {
        get => isTestExpressionThrowing ? throw new Exception() :  _testExpression;
        set => _testExpression = value;
    }
    
    public override Expression<Func<MockDataType1, bool>> ToExpression()
    {
        return isToExpressionThrowing ? throw new Exception() : TestExpression;
    }
}