using Moq;
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

    protected void SetupFirstAndSecondExpression(bool isFirstSatisfied, bool isSecondSatisfied)
    {
        var candidate1Name = isFirstSatisfied ? Candidate.Name : null;
        FirstExpression = firstMockDataType1 => firstMockDataType1.Name == candidate1Name;
        
        var candidate2Name  = isSecondSatisfied ? Candidate.Name : null;
        SecondExpression = secondMockDataType1 => secondMockDataType1.Name == candidate2Name;
        
        MockFirstSpecification.Setup(specification => specification.ToExpression()).Returns(FirstExpression);
        MockSecondSpecification.Setup(specification => specification.ToExpression()).Returns(SecondExpression);
        InitializeSystemUnderTest();
    }
    
    protected void VerifyMockSpecificationToExpressionCalls()
    {
        MockFirstSpecification.Verify(specification => specification.ToExpression(), Times.Once);
        MockFirstSpecification.VerifyNoOtherCalls();
        
        MockSecondSpecification.Verify(specification => specification.ToExpression(), Times.Once);
        MockSecondSpecification.VerifyNoOtherCalls();
    }
}