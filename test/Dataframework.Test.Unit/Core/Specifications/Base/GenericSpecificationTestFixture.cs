using System.Linq.Expressions;
using NUnit.Framework;
using Queueware.Dataframework.Core.Specifications;
using Queueware.Dataframework.Test.Common;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Base;

public abstract class GenericSpecificationTestFixture<TSpecification, TCandidate> : CommonTestBase
    where TCandidate : class where TSpecification : Specification<TCandidate>
{
    protected TCandidate Candidate { get; set; } = null!;

    protected Expression<Func<TCandidate, bool>> Expression { get; set; } = null!;

    protected TSpecification Specification { get; set; } = null!;

    [SetUp]
    public void SetUp()
    {
        Candidate = Create<TCandidate>();
        Expression = _ => true;
        Instantiate();
    }

    protected abstract void Instantiate();
}