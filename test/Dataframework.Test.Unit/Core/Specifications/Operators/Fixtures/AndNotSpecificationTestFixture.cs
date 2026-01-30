using Queueware.Dataframework.Core.Specifications.Operators;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Operators.Fixtures;

public class AndNotSpecificationTestFixture : BinaryOperatorSpecificationTestFixture<AndNotSpecification<MockDataType1>,
    string, MockDataType1>
{
    protected override void InitializeSystemUnderTest()
    {
        OperatorSpecification =
            new AndNotSpecification<MockDataType1>(MockSpecification.Object, MockOtherSpecification.Object);
    }
}