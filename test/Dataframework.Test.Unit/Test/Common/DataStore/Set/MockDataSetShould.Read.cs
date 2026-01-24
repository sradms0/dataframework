using System.Linq.Expressions;
using FluentAssertions;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Set;

public partial class MockDataSetShould
{
    [TestCaseRange(0, 2)]
    public async Task CountAsync(int expectedResult)
    {
        // Arrange
        DataSourceEntities = DataSourceEntities[..expectedResult];
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);

        int? result = null;

        // Act (define)
        var countAsync = async () => result = await MockDataSet.CountAsync(CancellationToken);

        // Assert
        await countAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
    }

    [TestCaseRange(0, 2)]
    public async Task CountAsync_By_Expression(int expectedResult)
    {
        // Arrange
        var name = Create<string>();
        DataSourceEntities[..expectedResult].ForEach(entity => entity.Name = name);
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);

        Expression<Func<MockDataType1, bool>> expression = entity => entity.Name == name;
        int? result = null;

        // Act (define)
        var countAsync = async () => result = await MockDataSet.CountAsync(expression, CancellationToken);

        // Assert
        await countAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
    }

    [TestCaseRange(0, 2)]
    public async Task FindAsync(int matchCount)
    {
        // Arrange
        var name = Create<string>();
        var entitiesToMatch = DataSourceEntities[..matchCount].OrderBy(entity => entity.Id).ToList();
        entitiesToMatch.ForEach(entity => entity.Name = name);
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);

        Expression<Func<MockDataType1, bool>> expression = entity => entity.Name == name;
        IEnumerable<MockDataType1>? result = null;

        // Act (define)
        var findAsync = async () =>
        {
            result = (await MockDataSet.FindAsync(expression, CancellationToken)).OrderBy(entity => entity.Id);
        };

        // Assert
        await findAsync.Should().NotThrowAsync();
        result.Should().BeEquivalentTo(entitiesToMatch);
    }

    [TestCaseRange(0, 2)]
    public async Task FirstOrDefaultAsync(int matchCount)
    {
        // Arrange
        var name = Create<string>();
        var entitiesToMatch = DataSourceEntities[..matchCount];
        entitiesToMatch.ForEach(entity => entity.Name = name);
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);

        Expression<Func<MockDataType1, bool>> expression = entity => entity.Name == name;
        var expectedResult = entitiesToMatch.FirstOrDefault(expression.Compile());
        MockDataType1? result = null;

        // Act (define)
        var firstOrDefaultAsync = async () =>
        {
            return result = await MockDataSet.FirstOrDefaultAsync(expression, CancellationToken);
        };

        // Assert
        await firstOrDefaultAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
    }
}