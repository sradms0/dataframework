using Queueware.Dataframework.Core.Specifications;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications;

public abstract class SpecificationTestFixture : GenericSpecificationTestFixture<TestSpecification, MockDataType1>
{
    protected override void Instantiate()
    {
        Specification = new TestSpecification();
    }
}