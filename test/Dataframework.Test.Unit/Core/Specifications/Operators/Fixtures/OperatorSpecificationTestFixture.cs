using System.Linq.Expressions;
using Moq;
using Queueware.Dataframework.Abstractions.Specifications;
using Queueware.Dataframework.Test.Common;
using Queueware.Dataframework.Test.Common.Mocks.Entities;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Operators.Fixtures;

public abstract class OperatorSpecificationTestFixture<TOperatorSpecification, TId, TCandidate> : CommonTestBase
    where TCandidate : MockGenericAbstractBaseType<TId>
{
    protected TCandidate Candidate { get; set; } = null!;

    protected Expression<Func<TCandidate, bool>> Expression { get; set; } = null!;
    
    protected Mock<ISpecification<TCandidate>> MockSpecification { get; set; } = null!;
    
    protected TOperatorSpecification OperatorSpecification { get; set; } = default!;

    public virtual void SetUp()
    {
        Candidate = Create<TCandidate>();
        Expression = _ => true;
        MockSpecification = new Mock<ISpecification<TCandidate>>();
        MockSpecification.Setup(specification => specification.ToExpression()).Returns(Expression);
        
        InitializeSystemUnderTest();
    }
    
    protected abstract void InitializeSystemUnderTest();
}