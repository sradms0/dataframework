using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Queueware.Dataframework.Abstractions.Specifications;
using Queueware.Dataframework.Test.Common.Mocks.Entities;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Operators.Fixtures;

public abstract class BinaryOperatorSpecificationTestFixture<TOperatorSpecification, TId, TCandidate> : 
    OperatorSpecificationTestFixture<TOperatorSpecification, TId, TCandidate>
    where TOperatorSpecification : ISpecification<TCandidate>
    where TCandidate : MockGenericAbstractBaseType<TId>
{
    protected abstract ExpressionBuildSpecifier SetExpressionBuildSpecifier { get; init; }

    protected Expression<Func<TCandidate, bool>> OtherTestExpression { get; set; } = null!;
    
    protected Mock<ISpecification<TCandidate>> MockOtherSpecification { get; set; } = null!;

    [SetUp]
    public override void SetUp()
    {
        OtherTestExpression = _ => true;
        MockOtherSpecification = new Mock<ISpecification<TCandidate>>();
        base.SetUp();
    }

    [Test, Combinatorial]
    public void Check_If_IsSatisfiedBy([Values(false, true)] bool isFirstSatisfied,
        [Values(false, true)] bool isSecondSatisfied)
    {
        // Arrange
        SetupPrimaryAndOtherExpression(isFirstSatisfied, isSecondSatisfied);
        var expectedResult = CreateExpectedSatisfactionResult(isFirstSatisfied, isSecondSatisfied);
        bool? result = null;
        
        // Act (define)
        var isSatisfiedBy = () => result = OperatorSpecification.IsSatisfiedBy(Candidate);

        // Assert
        isSatisfiedBy.Should().NotThrow();
        result.Should().Be(expectedResult);
        VerifyMockSpecificationToExpressionCalls();
    }
    
    [Test, Combinatorial]
    public void Translate_ToExpression([Values(false, true)] bool isFirstSatisfied,
        [Values(false, true)] bool isSecondSatisfied)
    {
        // Arrange
        SetupPrimaryAndOtherExpression(isFirstSatisfied, isSecondSatisfied);
        var expectedResult = CreateExpectedExpressionResult();
        Expression<Func<TCandidate, bool>>? result = null;

        var expectedCompiledFuncResult = expectedResult.Compile().Invoke(Candidate);
        bool? compiledFuncResult = null;
        
        // Act (define)
        var toExpression = () => result = OperatorSpecification.ToExpression();
        var compileAndRunFuncResult = () => compiledFuncResult = result?.Compile().Invoke(Candidate);

        // Assert
        toExpression.Should().NotThrow();
        result.Should().NotBeNull();
        result.ToString().Should().Be(expectedResult.ToString());
        
        compileAndRunFuncResult.Should().NotThrow();
        compiledFuncResult.Should().Be(expectedCompiledFuncResult);
        
        VerifyMockSpecificationToExpressionCalls();
    } 

    private void SetupPrimaryAndOtherExpression(bool isFirstSatisfied, bool isSecondSatisfied)
    {
        var candidate1Name = isFirstSatisfied ? Candidate.Name : null;
        TestExpression = firstMockDataType1 => firstMockDataType1.Name == candidate1Name;
        
        var candidate2Name  = isSecondSatisfied ? Candidate.Name : null;
        OtherTestExpression = secondMockDataType1 => secondMockDataType1.Name == candidate2Name;
        
        MockSpecification.Setup(specification => specification.ToExpression()).Returns(TestExpression);
        MockOtherSpecification.Setup(specification => specification.ToExpression()).Returns(OtherTestExpression);
        InitializeSystemUnderTest();
    }
    
    private void VerifyMockSpecificationToExpressionCalls()
    {
        MockSpecification.Verify(specification => specification.ToExpression(), Times.Once);
        MockSpecification.VerifyNoOtherCalls();
        
        MockOtherSpecification.Verify(specification => specification.ToExpression(), Times.Once);
        MockOtherSpecification.VerifyNoOtherCalls();
    }

    private Expression<Func<TCandidate, bool>> CreateExpectedExpressionResult()
    {
        var parameter = TestExpression.Parameters[0];
        var otherTestExpressionBody = new ReplaceParameterVisitor(OtherTestExpression.Parameters[0], parameter)
            .Visit(OtherTestExpression.Body);
        var expressionBody = CreateExpectedExpressionBody(otherTestExpressionBody);
        
        return Expression.Lambda<Func<TCandidate, bool>>(expressionBody, parameter);
    }

    protected enum ExpressionBuildSpecifier
    {
        Unspecified = 0,
        AndAlso = 1,
        AndAlsoNot = 2,
        Or = 3
    }

    private BinaryExpression CreateExpectedExpressionBody(Expression otherTestExpressionBody)
    {
        return SetExpressionBuildSpecifier switch
        {
            ExpressionBuildSpecifier.AndAlso => Expression.AndAlso(TestExpression.Body, otherTestExpressionBody),
            
            ExpressionBuildSpecifier.AndAlsoNot => Expression.AndAlso(TestExpression.Body,
                Expression.Not(otherTestExpressionBody)),
            
            ExpressionBuildSpecifier.Or => Expression.OrElse(TestExpression.Body, otherTestExpressionBody),
            
            ExpressionBuildSpecifier.Unspecified => throw new InvalidOperationException(),
            
            _ => throw new InvalidOperationException()
        };
    }

    private bool CreateExpectedSatisfactionResult(bool isFirstSatisfied, bool isSecondSatisfied)
    {
        return SetExpressionBuildSpecifier switch
        {
            ExpressionBuildSpecifier.AndAlso => isFirstSatisfied && isSecondSatisfied,
            ExpressionBuildSpecifier.AndAlsoNot => isFirstSatisfied && !isSecondSatisfied,
            ExpressionBuildSpecifier.Or => isFirstSatisfied || isSecondSatisfied,
            ExpressionBuildSpecifier.Unspecified => throw new InvalidOperationException(),
            _ => throw new InvalidOperationException()
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