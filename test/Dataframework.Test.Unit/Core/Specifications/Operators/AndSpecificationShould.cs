using System.Linq.Expressions;
using FluentAssertions;
using NUnit.Framework;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Operators;

public class AndSpecificationShould : AndSpecificationTestFixture
{
    [Test, Combinatorial]
    public void Check_If_Is_IsSatisfiedBy([Values(false, true)] bool isFirstSatisfied,
        [Values(false, true)] bool isSecondSatisfied)
    {
        // Arrange
        SetupFirstAndSecondExpression(isFirstSatisfied, isSecondSatisfied);
        var expectedResult = isFirstSatisfied && isSecondSatisfied;
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
        SetupFirstAndSecondExpression(isFirstSatisfied, isSecondSatisfied);
        var expressionBody = Expression.AndAlso(FirstExpression.Body, SecondExpression.Body);
        var parameter = FirstExpression.Parameters[0];
        var expectedResult = Expression.Lambda<Func<MockDataType1, bool>>(expressionBody, parameter);
        Expression<Func<MockDataType1, bool>>? result = null;

        var expectedCompiledFuncResult = FirstExpression.Compile().Invoke(Candidate) && 
                                         SecondExpression.Compile().Invoke(Candidate);
        bool? compiledFuncResult = null;
        
        // Act (define)
        var toExpression = () => result = OperatorSpecification.ToExpression();
        var compileAndRunFuncResult = () => compiledFuncResult = result?.Compile().Invoke(Candidate);

        // Assert
        toExpression.Should().NotThrow();
        result.Should().BeEquivalentTo(expectedResult);
        
        compileAndRunFuncResult.Should().NotThrow();
        compiledFuncResult.Should().Be(expectedCompiledFuncResult);
        
        VerifyMockSpecificationToExpressionCalls();
    } 
}