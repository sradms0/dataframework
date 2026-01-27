using System.Linq.Expressions;
using NUnit.Framework;
using Queueware.Dataframework.Core.Specifications;
using Queueware.Dataframework.Test.Common;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core;

public class ExpressionSpecificationTestFixture : CommonTestBase
{
    protected MockDataType1 MockDataType1 { get; set; } = null!;

    protected Expression<Func<MockDataType1, bool>> Expression { get; set; } = null!;

    protected ExpressionSpecification<MockDataType1> ExpressionSpecification { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        MockDataType1 = Create<MockDataType1>();
        Expression = _ => true; 
        Instantiate();
    }

    protected void Instantiate()
    {
        ExpressionSpecification = new ExpressionSpecification<MockDataType1>(Expression);
    }
}