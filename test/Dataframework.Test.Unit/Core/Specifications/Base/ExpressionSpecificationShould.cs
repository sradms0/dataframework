using System.Linq.Expressions;
using FluentAssertions;
using NUnit.Framework;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;
using Queueware.Dataframework.Test.Unit.Core.Specifications.Base.Fixtures;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Base;

public class ExpressionSpecificationShould : ExpressionSpecificationTestFixture
{
    [TestCaseShift(2)]
    public void Check_If_Is_IsSatisfiedBy(bool isMatch, bool isNull)
    {
        // Arrange
        var name = Candidate.Name;
        if (!isNull)
        {
            Candidate.Name = isMatch ? name : null;
        }
        else
        {
            Candidate = null!;
        }
        Expression = mockDataType1 => mockDataType1.Name == name;
        Instantiate();

        bool? expectedResult = !isNull ? isMatch : null;
        bool? result = null;

        // Act (define)
        var isSatisfied = () => result = Specification.IsSatisfiedBy(Candidate);

        // Assert
        var isSatisfiedShould = isSatisfied.Should();
        if (isNull)
        {
            isSatisfiedShould.Throw();
        }
        else
        {
            isSatisfied.Should().NotThrow();
        }
        result.Should().Be(expectedResult);
    }
    
    [Test]
    public void Throw_When_Expression_Is_Null()
    {
        // Arrange
        Expression = null!;
        const string ExpectedMessage = "Value cannot be null. (Parameter 'expression')";

        // Act (define)
        var instantiate = Instantiate;

        // Assert
        instantiate.Should().Throw<ArgumentNullException>().WithMessage(ExpectedMessage);
    }

    [Test]
    public void Translate_ToExpression()
    {
        // Arrange
        Expression = mockDataType1 => mockDataType1.Name == Candidate.Name;
        Instantiate();
        Expression<Func<MockDataType1, bool>>? result = null;
        const bool ExpectedCompiledFuncResult = true;
        bool? compiledFuncResult = null;
        
        // Act (define)
        var toExpression = () => result = Specification.ToExpression();
        var compileAndRunFunc = () => compiledFuncResult = result?.Compile()(Candidate);

        // Assert
        toExpression.Should().NotThrow();
        result.Should().Be(Expression);
        compileAndRunFunc.Should().NotThrow();
        compiledFuncResult.Should().Be(ExpectedCompiledFuncResult);
    }
}