using FluentAssertions;
using NUnit.Framework;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Set;

public partial class MockDataSetShould
{
    [TestCase(false)]
    [TestCase(true)]
    public async Task AddAsync_Of_One(bool isNull)
    {
        // Arrange
        var entityToAdd = !isNull ? DataSourceEntities.First() : null;
        MockDataType1? entityAdded = null;

        // Act (define)
        var addAsync = async () => await MockDataSet.AddAsync(entityToAdd!, CancellationToken);
        var getAddedEntity = () =>
        {
            entityAdded =
                MockDataSource.FirstOrDefault<string, MockDataType1>(entity => entity.Id == entityToAdd?.Id);
        };

        // Assert
        await addAsync.Should().NotThrowAsync();
        getAddedEntity.Should().NotThrow();
        var entityAddedShould = entityAdded.Should();
        if (!isNull)
        {
            entityAddedShould.NotBeSameAs(entityToAdd);
            entityAddedShould.BeEquivalentTo(entityToAdd);
        }
        else
        {
            entityAddedShould.BeNull();
        }
    }

    [Test]
    public async Task AddAsync_Of_One_When_Not_Null()
    {
        // Arrange
        IEnumerable<MockDataType1>? addedResults = null;

        // Act (define)
        var addAsync = async () => await MockDataSet.AddAsync((MockDataType1?)null!, CancellationToken);
        var getNullEntities = () => addedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await addAsync.Should().NotThrowAsync();
        getNullEntities.Should().NotThrow();
        addedResults.Should().BeEmpty();
    }

    [TestCaseRange(0, 2)]
    public async Task AddAsync_Of_Many(int addCount)
    {
        // Arrange
        var entitiesToAdd = DataSourceEntities[..addCount];
        IEnumerable<MockDataType1>? entitiesAdded = null;

        // Act (define)
        var addAsync = async () => await MockDataSet.AddAsync(entitiesToAdd, CancellationToken);
        var getAddedEntities = () =>
        {
            entitiesAdded = MockDataSource.Where<string, MockDataType1>(entity => entitiesToAdd.Contains(entity));
        };

        // Assert
        await addAsync.Should().NotThrowAsync();
        getAddedEntities.Should().NotThrow();
        entitiesAdded.Should().BeEquivalentTo(entitiesToAdd);
    }

    [Test]
    public async Task AddAsync_Of_Many_When_Not_Null()
    {
        // Arrange
        IEnumerable<MockDataType1>? addedResults = null;

        // Act (define)
        var addAsync = async () => await MockDataSet.AddAsync((IEnumerable<MockDataType1>?)null!, CancellationToken);
        var getNullEntities = () => addedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await addAsync.Should().NotThrowAsync();
        getNullEntities.Should().NotThrow();
        addedResults.Should().BeEmpty();
    }

    [TestCaseShift(2, true)]
    public async Task AddAsync_Of_Many_When_Data_Not_Null(bool isOneNullAdd, bool isManyNullAdd)
    {
        // Arrange
        var nullDataToAdd = new List<MockDataType1?>(isOneNullAdd ? [null] : [null, null]);
        IEnumerable<MockDataType1>? addedResults = null;

        // Act (define)
        var addAsync = async () => await MockDataSet.AddAsync(nullDataToAdd!, CancellationToken);
        var getNullEntities = () => addedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await addAsync.Should().NotThrowAsync();
        getNullEntities.Should().NotThrow();
        addedResults.Should().BeEmpty();
    }
}