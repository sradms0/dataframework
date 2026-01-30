using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Queueware.Dataframework.Abstractions.Specifications;
using Queueware.Dataframework.Test.Common.Mocks.Entities;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Operators.Fixtures;

public abstract class BinaryOperatorSpecificationTestFixture<TOperatorSpecification, TId, TCandidate> : 
    OperatorSpecificationTestFixture<TOperatorSpecification, TId, TCandidate>
    where TCandidate : MockGenericAbstractBaseType<TId>
{
    protected Expression<Func<TCandidate, bool>> OtherExpression { get; set; } = null!;
    
    protected Mock<ISpecification<TCandidate>> MockOtherSpecification { get; set; } = null!;

    [SetUp]
    public override void SetUp()
    {
        OtherExpression = _ => true;
        MockOtherSpecification = new Mock<ISpecification<TCandidate>>();
        base.SetUp();
    }

    protected void SetupPrimaryAndOtherExpression(bool isFirstSatisfied, bool isSecondSatisfied)
    {
        var candidate1Name = isFirstSatisfied ? Candidate.Name : null;
        Expression = firstMockDataType1 => firstMockDataType1.Name == candidate1Name;
        
        var candidate2Name  = isSecondSatisfied ? Candidate.Name : null;
        OtherExpression = secondMockDataType1 => secondMockDataType1.Name == candidate2Name;
        
        MockSpecification.Setup(specification => specification.ToExpression()).Returns(Expression);
        MockOtherSpecification.Setup(specification => specification.ToExpression()).Returns(OtherExpression);
        InitializeSystemUnderTest();
    }
    
    protected void VerifyMockSpecificationToExpressionCalls()
    {
        MockSpecification.Verify(specification => specification.ToExpression(), Times.Once);
        MockSpecification.VerifyNoOtherCalls();
        
        MockOtherSpecification.Verify(specification => specification.ToExpression(), Times.Once);
        MockOtherSpecification.VerifyNoOtherCalls();
    }
}