using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Queueware.Dataframework.Abstractions.Specifications;
using Queueware.Dataframework.Test.Common;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Operators;

public abstract class OperatorSpecificationTestFixture<TOperatorSpecification, TCandidate> : CommonTestBase where TCandidate : class
{
    protected TCandidate Candidate { get; set; } = null!;

    protected Expression<Func<TCandidate, bool>> FirstExpression { get; set; } = null!;

    protected Expression<Func<TCandidate, bool>> SecondExpression { get; set; } = null!;
    
    protected Mock<ISpecification<TCandidate>> MockFirstSpecification { get; set; } = null!;

    protected Mock<ISpecification<TCandidate>> MockSecondSpecification { get; set; } = null!;
    
    protected TOperatorSpecification OperatorSpecification { get; set; } = default!;

    [SetUp]
    public void SetUp()
    {
        Candidate = Create<TCandidate>();
        
        FirstExpression = _ => true;
        SecondExpression = _ => true;
        
        MockFirstSpecification = new Mock<ISpecification<TCandidate>>();
        MockSecondSpecification = new Mock<ISpecification<TCandidate>>();
        
        MockFirstSpecification.Setup(specification => specification.ToExpression()).Returns(FirstExpression);
        MockSecondSpecification.Setup(specification => specification.ToExpression()).Returns(SecondExpression);
        
        InitializeSystemUnderTest();
    }
    
    protected abstract void InitializeSystemUnderTest();
}