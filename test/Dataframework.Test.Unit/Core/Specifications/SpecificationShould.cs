using FluentAssertions;
using NUnit.Framework;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications;

public class SpecificationShould : SpecificationTestFixture
{
    [Test,Combinatorial]
    public void Check_If_IsSatisfiedBy_During_Exceptions([Values(false, true)] bool isToExpressionThrowing, [Values(false, true)] bool isTestExpressionThrowing)
    {
        // Arrange
        Specification.IsToExpressionThrowing = isToExpressionThrowing;
        Specification.IsTestExpressionThrowing = isTestExpressionThrowing;
        
        // Act (define)
        var isSatisfiedBy = () => Specification.IsSatisfiedBy(Candidate);
        
        // Assert
        var isSatisfiedByShould = isSatisfiedBy.Should();

        if (isTestExpressionThrowing || isToExpressionThrowing)
        {
            isSatisfiedByShould.Throw();
        }
        else
        {
            isSatisfiedByShould.NotThrow();
        }
    }
    
    [Test,Combinatorial]
    public void Check_If_IsSatisfiedBy_When_Expression_Is_Null([Values(false, true)] bool isExpressNull)
    {
        // Arrange
        Specification.TestExpression = isExpressNull ? null! : Specification.TestExpression;
        
        // Act (define)
        var isSatisfiedBy = () => Specification.IsSatisfiedBy(Candidate);
        
        // Assert
        isSatisfiedBy.Should().NotThrow();
    }
}