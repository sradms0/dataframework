using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Queueware.Dataframework.Core.Specifications.Operators;
using Queueware.Dataframework.Test.Unit.Core.Specifications.Operators.Fixtures;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Operators;

public class NotSpecificationShould : OperatorSpecificationTestFixture<NotSpecification<MockDataType1>, string, MockDataType1>
{
    [SetUp]
    public new void SetUp()  => base.SetUp();
    
    [Test, Combinatorial]
    public void Check_If_IsSatisfiedBy([Values(false, true)] bool isSatisfied)
    {
        // Arrange
        var name = Candidate.Name;
        if (isSatisfied)
        {
            name = Create<string>();
        }
        TestExpression = mockDataType1 => mockDataType1.Name == name;
        MockSpecification.Setup(specification => specification.ToExpression()).Returns(TestExpression);
        InitializeSystemUnderTest();
        
        bool? result = null;
        
        // Act (define)
        var isSatisfiedBy = () => result = OperatorSpecification.IsSatisfiedBy(Candidate);
        
        // Assert
        isSatisfiedBy.Should().NotThrow();
        result.Should().Be(isSatisfied);
        MockSpecification.Verify(specification => specification.ToExpression(), Times.Once);
        MockSpecification.VerifyNoOtherCalls();
    }

    [Test, Combinatorial]
    public void Translate_ToExpression([Values(false, true)] bool isSatisfied)
    {
        // Arrange
        var name = Candidate.Name;
        if (isSatisfied)
        {
            name = Create<string>();
        }
        TestExpression = mockDataType1 => mockDataType1.Name == name;
        MockSpecification.Setup(specification => specification.ToExpression()).Returns(TestExpression);
        InitializeSystemUnderTest();
        
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
        
        MockSpecification.Verify(specification => specification.ToExpression(), Times.Once);
        MockSpecification.VerifyNoOtherCalls();
    }
    
    protected override void InitializeSystemUnderTest()
    {
        OperatorSpecification = new NotSpecification<MockDataType1>(MockSpecification.Object);
    }
}