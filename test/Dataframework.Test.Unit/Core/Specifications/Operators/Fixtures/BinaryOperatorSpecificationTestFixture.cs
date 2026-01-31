using System.ComponentModel;
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
    protected Expression<Func<TCandidate, bool>> OtherTestExpression { get; set; } = null!;
    
    protected Mock<ISpecification<TCandidate>> MockOtherSpecification { get; set; } = null!;

    [SetUp]
    public override void SetUp()
    {
        OtherTestExpression = _ => true;
        MockOtherSpecification = new Mock<ISpecification<TCandidate>>();
        base.SetUp();
    }

    protected void SetupPrimaryAndOtherExpression(bool isFirstSatisfied, bool isSecondSatisfied)
    {
        var candidate1Name = isFirstSatisfied ? Candidate.Name : null;
        TestExpression = firstMockDataType1 => firstMockDataType1.Name == candidate1Name;
        
        var candidate2Name  = isSecondSatisfied ? Candidate.Name : null;
        OtherTestExpression = secondMockDataType1 => secondMockDataType1.Name == candidate2Name;
        
        MockSpecification.Setup(specification => specification.ToExpression()).Returns(TestExpression);
        MockOtherSpecification.Setup(specification => specification.ToExpression()).Returns(OtherTestExpression);
        InitializeSystemUnderTest();
    }
    
    protected void VerifyMockSpecificationToExpressionCalls()
    {
        MockSpecification.Verify(specification => specification.ToExpression(), Times.Once);
        MockSpecification.VerifyNoOtherCalls();
        
        MockOtherSpecification.Verify(specification => specification.ToExpression(), Times.Once);
        MockOtherSpecification.VerifyNoOtherCalls();
    }

    protected Expression<Func<TCandidate, bool>> CreateExpectedExpressionResult(ExpressionBuildSpecifier expressionBuildSpecifier)
    {
        var parameter = TestExpression.Parameters[0];
        var otherTestExpressionBody = new ReplaceParameterVisitor(OtherTestExpression.Parameters[0], parameter)
            .Visit(OtherTestExpression.Body);
        var expressionBody = CreateExpectedExpressionBody(expressionBuildSpecifier, otherTestExpressionBody);
        
        return Expression.Lambda<Func<TCandidate, bool>>(expressionBody, parameter);
    }

    protected enum ExpressionBuildSpecifier
    {
        Unspecified = 0,
        AndAlso = 1,
        AndAlsoNot = 2
    }

    private BinaryExpression CreateExpectedExpressionBody(ExpressionBuildSpecifier expressionBuildSpecifier,
        Expression otherTestExpressionBody)
    {
        return expressionBuildSpecifier switch
        {
            ExpressionBuildSpecifier.AndAlso => Expression.AndAlso(TestExpression.Body, otherTestExpressionBody),
            ExpressionBuildSpecifier.AndAlsoNot => Expression.AndAlso(TestExpression.Body,
                Expression.Not(otherTestExpressionBody)),
            ExpressionBuildSpecifier.Unspecified => throw new InvalidOperationException(),
            _ => throw new ArgumentOutOfRangeException(nameof(expressionBuildSpecifier), expressionBuildSpecifier, null)
        };
    }
    
    private sealed class ReplaceParameterVisitor(ParameterExpression fromParameterExpression,
        ParameterExpression toParameterExpression) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression currentParameterExpression)
        {
            return ReferenceEquals(currentParameterExpression, fromParameterExpression)
                ? toParameterExpression
                : base.VisitParameter(currentParameterExpression);
        }
    }
}