using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Specifications.Base;

public abstract class SpecificationTestFixture : GenericSpecificationTestFixture<TestSpecification, MockDataType1>
{
    protected override void Instantiate()
    {
        Specification = new TestSpecification();
    }
}