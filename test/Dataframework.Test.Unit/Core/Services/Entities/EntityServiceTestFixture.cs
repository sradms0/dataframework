using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Queueware.Dataframework.Abstractions.Repositories;
using Queueware.Dataframework.Core.Services.Entities;
using Queueware.Dataframework.Test.Common;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Services.Entities;

public abstract class EntityServiceTestFixture : CommonTestBase
{
    protected CancellationToken CancellationToken { get; set; }

    protected MockDataType1 Entity { get; set; } = null!;

    protected List<MockDataType1> Entities { get; set; } = null!;

    protected readonly Expression<Func<MockDataType1, bool>> FindExpression = _ => true;

    protected Expression<Func<MockDataType1, string>> SetStringExpression = entity => entity.Name!;

    protected Mock<IRepository<string, MockDataType1>> MockRepository { get; set; } = null!;

    protected EntityService<string, MockDataType1> EntityService { get; set; } = null!;

    [SetUp]
    public void SetUp()
    {
        CancellationToken = CancellationToken.None;
        Entity = Create<MockDataType1>();
        Entities = CreateMany<MockDataType1>().ToList();
        
        MockRepository = new Mock<IRepository<string, MockDataType1>>();
        EntityService = new EntityService<string, MockDataType1>();
    }
}