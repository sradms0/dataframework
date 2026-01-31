using Queueware.Dataframework.Core.Specifications.Operators;
using Queueware.Dataframework.Test.Unit.Core.Specifications.Operators.Fixtures;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Operators;

public class AndSpecificationShould : BinaryOperatorSpecificationTestFixture<
    AndSpecification<MockDataType1>, string, MockDataType1>
{
    protected override ExpressionBuildSpecifier SetExpressionBuildSpecifier { get; init; } =
        ExpressionBuildSpecifier.AndAlso;

    protected override void InitializeSystemUnderTest()
    {
        OperatorSpecification =
            new AndSpecification<MockDataType1>(MockSpecification.Object, MockOtherSpecification.Object);
    }
}