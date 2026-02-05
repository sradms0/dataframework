using Moq;
using NUnit.Framework;
using Queueware.Dataframework.Core.Specifications.Operators;
using Queueware.Dataframework.Test.Unit.Core.Specifications.Operators.Fixtures;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Operators.Unary;

public abstract class NotSpecificationTestFixture : OperatorSpecificationTestFixture<NotSpecification<MockDataType1>, string, MockDataType1> 
{
    [SetUp]
    public new void SetUp()  => base.SetUp();
    
    protected override void InitializeSystemUnderTest()
    {
        OperatorSpecification = new NotSpecification<MockDataType1>(MockSpecification.Object);
    }

    protected void Arrange(bool isSatisfied)
    {
        var name = Candidate.Name;
        if (isSatisfied)
        {
            name = Create<string>();
        }
        TestExpression = mockDataType1 => mockDataType1.Name == name;
        MockSpecification.Setup(specification => specification.ToExpression()).Returns(TestExpression);
        InitializeSystemUnderTest();
    }

    protected void VerifyMockSpecificationToExpressionCalls()
    {
        MockSpecification.Verify(specification => specification.ToExpression(), Times.Once);
        MockSpecification.VerifyNoOtherCalls();
    }
}