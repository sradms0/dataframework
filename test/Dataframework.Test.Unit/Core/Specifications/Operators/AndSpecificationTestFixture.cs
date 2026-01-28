using Queueware.Dataframework.Core.Specifications;
using Queueware.Dataframework.Core.Specifications.Operators;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Operators;

public abstract class AndSpecificationTestFixture : OperatorSpecificationTestFixture<AndSpecification<MockDataType1>, MockDataType1>
{
    protected override void InitializeSystemUnderTest()
    {
        OperatorSpecification =
            new AndSpecification<MockDataType1>(MockFirstSpecification.Object, MockSecondSpecification.Object);
    }
}