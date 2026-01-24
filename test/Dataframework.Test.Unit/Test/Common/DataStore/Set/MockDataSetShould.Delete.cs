using FluentAssertions;
using NUnit.Framework;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;
using Queueware.Dataframework.Test.Common.Utils;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Set;

public partial class MockDataSetShould
{
    [TestCaseShift(3)]
    public void Return_AsQueryable(bool isOne, bool isMany, bool isAll)
    {
        // Arrange
        var expectedResult = TestArrangementHelper.GetRangeFrom(DataSourceEntities, isOne, isMany, isAll).ToList();
        MockDataSource.Add<string, MockDataType1>(expectedResult);
        IEnumerable<MockDataType1>? result = null;

        // Act (define)
        var asQueryable = () => result = MockDataSet.AsQueryable();

        // Assert
        asQueryable.Should().NotThrow();
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task RemoveAsync_Of_One(bool doesExist)
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        var entityToRemove = doesExist
            ? DataSourceEntities[new Random().Next(DataSourceEntities.Count)]
            : Create<MockDataType1>();
        MockDataType1? removalResult = null;

        // Act (define)
        var removeAsync = async () => await MockDataSet.RemoveAsync(entityToRemove, CancellationToken);
        var get = () => removalResult = MockDataSource.Get<string, MockDataType1>(entityToRemove.Id);

        // Assert
        await removeAsync.Should().NotThrowAsync();
        get.Should().NotThrow();
        removalResult.Should().BeNull();
    }

    [Test]
    public async Task RemoveAsync_When_Not_Null()
    {
        // Arrange
        IEnumerable<MockDataType1>? removalResult = null;

        // Act (define)
        var removeAsync = () => MockDataSet.RemoveAsync((MockDataType1?)null!, CancellationToken);
        var firstOrDefault = () => MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await removeAsync.Should().NotThrowAsync();
        firstOrDefault.Should().NotThrow();
        removalResult.Should().BeNull();
    }

    [TestCaseShift(3)]
    public async Task RemoveAsync_Of_Many(bool isRemoveOne, bool isRemoveMany, bool isRemoveAll)
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        var dataToRemove = TestArrangementHelper
            .GetRangeFrom(DataSourceEntities, isRemoveOne, isRemoveMany, isRemoveAll).ToList();
        IEnumerable<MockDataType1>? removedResult = null;

        // Act (define)
        var removeAsync = async () => await MockDataSet.RemoveAsync(dataToRemove, CancellationToken);
        var where = () =>
            removedResult = MockDataSource.Where<string, MockDataType1>(data => dataToRemove.Contains(data));

        // Assert
        await removeAsync.Should().NotThrowAsync();
        where.Should().NotThrow();
        removedResult.Should().BeEmpty();
    }

    [Test]
    public async Task RemoveAsync_Of_Many_When_Not_Null()
    {
        // Arrange
        IEnumerable<MockDataType1>? removedResults = null;

        // Act (define)
        var removeAsync = async () =>
            await MockDataSet.RemoveAsync((IEnumerable<MockDataType1>?)null!, CancellationToken);
        var where = () => removedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await removeAsync.Should().NotThrowAsync();
        where.Should().NotThrow();
        removedResults.Should().BeEmpty();
    }

    [TestCaseShift(2, true)]
    public async Task RemoveAsync_Of_Many_When_Data_Not_Null(bool isOneNullRemove, bool isManyNullRemove)
    {
        // Arrange
        var nullDataToRemove = new List<MockDataType1?>(isOneNullRemove ? [null] : [null, null]);
        IEnumerable<MockDataType1>? removedResults = null;

        // Act (define)
        var removeAsync = async () => await MockDataSet.RemoveAsync(nullDataToRemove!, CancellationToken);
        var where = () => removedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await removeAsync.Should().NotThrowAsync();
        where.Should().NotThrow();
        removedResults.Should().BeEmpty();
    }
}