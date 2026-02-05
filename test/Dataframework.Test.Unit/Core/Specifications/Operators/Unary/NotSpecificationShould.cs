using System.Linq.Expressions;
using FluentAssertions;
using NUnit.Framework;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Operators.Unary;

public class NotSpecificationShould : NotSpecificationTestFixture
{
    [Test, Combinatorial]
    public void Check_If_IsSatisfiedBy([Values(false, true)] bool isSatisfied)
    {
        // Arrange
        Arrange(isSatisfied);
        bool? result = null;
        
        // Act (define)
        var isSatisfiedBy = () => result = OperatorSpecification.IsSatisfiedBy(Candidate);
        
        // Assert
        isSatisfiedBy.Should().NotThrow();
        result.Should().Be(isSatisfied);
        VerifyMockSpecificationToExpressionCalls();
    }

    [Test, Combinatorial]
    public void Translate_ToExpression([Values(false, true)] bool isSatisfied)
    {
        // Arrange
        Arrange(isSatisfied);
        var expectedExpressionBody = Expression.Not(TestExpression.Body);
        var expectedExpressionParameter = TestExpression.Parameters[0];
        var expectedResult = Expression
            .Lambda<Func<MockDataType1, bool>>(expectedExpressionBody, expectedExpressionParameter);
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