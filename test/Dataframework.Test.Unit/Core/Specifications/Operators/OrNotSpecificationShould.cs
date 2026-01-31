using Queueware.Dataframework.Core.Specifications.Operators;
using Queueware.Dataframework.Test.Unit.Core.Specifications.Operators.Fixtures;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Operators;

public class OrNotSpecificationShould : BinaryOperatorSpecificationTestFixture<OrNotSpecification<MockDataType1>, string, MockDataType1>
{
    protected override ExpressionBuildSpecifier SetExpressionBuildSpecifier { get; init; } =
        ExpressionBuildSpecifier.OrNot;
    
    protected override void InitializeSystemUnderTest()
    {
        OperatorSpecification =
            new OrNotSpecification<MockDataType1>(MockSpecification.Object, MockOtherSpecification.Object);
    }
}