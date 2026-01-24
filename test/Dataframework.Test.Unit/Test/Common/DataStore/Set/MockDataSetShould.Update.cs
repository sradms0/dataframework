using System.Linq.Expressions;
using FluentAssertions;
using Force.DeepCloner;
using NUnit.Framework;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Set;

public partial class MockDataSetShould
{
    [TestCase(false, false)]
    [TestCase(true, false)]
    [TestCase(true, true)]
    public async Task UpdateAsync_Of_One(bool doesExist, bool isUpdate)
    {
        // Arrange
        var dataSourceEntities = DataSourceEntities;
        var entityToUpdate = DataSourceEntities.First();
        if (!doesExist)
        {
            dataSourceEntities = dataSourceEntities[1..];
        }

        MockDataSource.Add<string, MockDataType1>(dataSourceEntities);

        if (isUpdate)
        {
            entityToUpdate.Name = Create<string>();
        }

        var expectedUpdatedResult = doesExist ? entityToUpdate : null;
        MockDataType1? updatedResult = null;

        // Act (define)
        var updateAsync = async () => await MockDataSet.UpdateAsync(entityToUpdate, CancellationToken);
        var getUpdatedEntity = () => updatedResult = MockDataSource.Get<string, MockDataType1>(entityToUpdate.Id);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        getUpdatedEntity.Should().NotThrow();
        updatedResult.Should().BeEquivalentTo(expectedUpdatedResult);
    }

    [Test]
    public async Task UpdateAsync_Of_One_When_Not_Null()
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        IEnumerable<MockDataType1>? updatedResults = null;

        // Act (define)
        var updateAsync = async () => await MockDataSet.UpdateAsync((MockDataType1?)null!, CancellationToken);
        var getNullEntities = () => updatedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        getNullEntities.Should().NotThrow();
        updatedResults.Should().BeEmpty();
    }

    [TestCaseShift(2)]
    [TestCase(true, true)]
    public async Task UpdateAsync_Of_One_By_Field_Value(bool isUpdate, bool doesExist)
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        var entityToUpdate = DataSourceEntities.First();
        var updatedNameValue = entityToUpdate.Name;
        Expression<Func<MockDataType1, string?>> updateFieldExpression = entity => entity.Name;
        if (doesExist && isUpdate)
        {
            updatedNameValue = Create<string>();
            entityToUpdate.Name = updatedNameValue;
        }
        else if (!doesExist)
        {
            entityToUpdate = Create<MockDataType1>();
        }
        MockDataType1? updatedResult = null;

        // Act (define)
        var updateAsync = async() => await MockDataSet.UpdateAsync(entityToUpdate, updateFieldExpression, updatedNameValue,CancellationToken);
        var get = () => updatedResult = MockDataSource.Get<string, MockDataType1>(entityToUpdate.Id);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        get.Should().NotThrow();
        var updatedResultShould = updatedResult.Should();
        if (doesExist)
        {
            updatedResultShould.BeEquivalentTo(entityToUpdate);
        }
        else
        {
            updatedResultShould.BeNull();
        }
    }

    [Test]
    public async Task Update_One_By_Field_Value_As_Copy()
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        var entityToUpdate = DataSourceEntities.First();
        var updatedNameValue = Create<string>();
        entityToUpdate.Name = updatedNameValue;
        Expression<Func<MockDataType1, string?>> updateFieldExpression = entity => entity.Name;

        MockDataType1? updatedResult = null;

        // Act (define)
        var updateAsync = async () => await MockDataSet.UpdateAsync(entityToUpdate, updateFieldExpression, updatedNameValue, CancellationToken);
        var get = () => updatedResult = MockDataSource.Get<string, MockDataType1>(entityToUpdate.Id);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        get.Should().NotThrow();
        updatedResult.Should().NotBeSameAs(entityToUpdate);
        updatedResult.Should().BeEquivalentTo(entityToUpdate);
    }

    [Test, Combinatorial]
    public async Task Update_One_By_Field_Value_When_Not_Null([Values(false, true)] bool isIdNull, [Values(false, true)] bool isExpressionNull, [Values(false, true)] bool isUpdateValueNull)
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        var originalEntity = DataSourceEntities.First();
        var id = originalEntity.Id;
        var entityToUpdate = originalEntity.DeepClone();
        var updatedNameValue = isUpdateValueNull ? null : Create<string>();
        entityToUpdate.Name = updatedNameValue;
        Expression<Func<MockDataType1, string?>> updateFieldExpression = (isExpressionNull ? null : entity => entity.Name)!;

        if (isIdNull)
        {
            entityToUpdate.Id = null!;
        }
        var expectedUpdatedResult = !isIdNull && !isExpressionNull ? entityToUpdate : originalEntity;
        MockDataType1? updatedResult = null;

        // Act (define)
        var updateAsync = async () => await MockDataSet.UpdateAsync(entityToUpdate, updateFieldExpression, updatedNameValue, CancellationToken);
        var get = () => updatedResult = MockDataSource.Get<string, MockDataType1>(id);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        get.Should().NotThrow();
        updatedResult.Should().BeEquivalentTo(expectedUpdatedResult);
    }

    [TestCaseRange(0, 2)]
    public async Task UpdateAsync_Of_Many(int updateCount)
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        var entitiesToUpdate = DataSourceEntities[..updateCount];
        IEnumerable<MockDataType1>? entitiesUpdated = null;

        // Act (define)
        var updateAsync = async () => await MockDataSet.UpdateAsync(entitiesToUpdate, CancellationToken);
        var getUpdatedEntities = () =>
        {
            entitiesUpdated = MockDataSource.Where<string, MockDataType1>(entity => entitiesToUpdate.Contains(entity));
        };

        // Assert
        await updateAsync.Should().NotThrowAsync();
        getUpdatedEntities.Should().NotThrow();
        entitiesUpdated.Should().BeEquivalentTo(entitiesToUpdate);
    }

    [Test]
    public async Task UpdateAsync_Of_Many_When_Not_Null()
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        IEnumerable<MockDataType1>? updatedResults = null;

        // Act (define)
        var updateAsync = async () => await MockDataSet.UpdateAsync((IEnumerable<MockDataType1>?)null!, CancellationToken);
        var getNullEntities = () => updatedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        getNullEntities.Should().NotThrow();
        updatedResults.Should().BeEmpty();
    }

    [TestCaseShift(2, true)]
    public async Task UpdateAsync_Of_Many_When_Data_Not_Null(bool isOneNullUpdate, bool isManyNullUpdate)
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        var nullDataToUpdate = new List<MockDataType1?>(isOneNullUpdate ? [null] : [null, null]);
        IEnumerable<MockDataType1>? updatedResults = null;

        // Act (define)
        var updateAsync = async () => await MockDataSet.UpdateAsync(nullDataToUpdate!, CancellationToken);
        var getNullEntities = () => updatedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        getNullEntities.Should().NotThrow();
        updatedResults.Should().BeEmpty();
    }
}