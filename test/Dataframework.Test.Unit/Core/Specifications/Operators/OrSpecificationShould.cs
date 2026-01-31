using Queueware.Dataframework.Core.Specifications.Operators;
using Queueware.Dataframework.Test.Unit.Core.Specifications.Operators.Fixtures;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Operators;

public class OrSpecificationShould : BinaryOperatorSpecificationTestFixture<OrSpecification<MockDataType1>, string, MockDataType1>
{
    protected override ExpressionBuildSpecifier SetExpressionBuildSpecifier { get; init; } =
        ExpressionBuildSpecifier.Or;
    
    protected override void InitializeSystemUnderTest()
    {
        OperatorSpecification =
            new OrSpecification<MockDataType1>(MockSpecification.Object, MockOtherSpecification.Object);
    }
}