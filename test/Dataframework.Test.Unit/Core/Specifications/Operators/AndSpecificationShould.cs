using System.Linq.Expressions;
using FluentAssertions;
using NUnit.Framework;
using Queueware.Dataframework.Test.Unit.Core.Specifications.Operators.Fixtures;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Operators;

public class AndSpecificationShould : AndSpecificationTestFixture
{
    [Test, Combinatorial]
    public void Check_If_Is_IsSatisfiedBy([Values(false, true)] bool isFirstSatisfied,
        [Values(false, true)] bool isSecondSatisfied)
    {
        // Arrange
        SetupPrimaryAndOtherExpression(isFirstSatisfied, isSecondSatisfied);
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
        SetupPrimaryAndOtherExpression(isFirstSatisfied, isSecondSatisfied);
        var expectedResult = CreateExpectedExpressionResult(ExpressionBuildSpecifier.AndAlso);
        Expression<Func<MockDataType1, bool>>? result = null;

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
}