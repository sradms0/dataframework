using Queueware.Dataframework.Core.Specifications;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Base.Fixtures;

public abstract class ExpressionSpecificationTestFixture 
    : GenericSpecificationTestFixture<ExpressionSpecification<MockDataType1>, MockDataType1>
{
    protected override void Instantiate()
    {
        Specification = new ExpressionSpecification<MockDataType1>(Expression);
    }
}