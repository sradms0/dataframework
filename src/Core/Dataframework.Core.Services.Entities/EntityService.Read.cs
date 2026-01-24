using System.Linq.Expressions;

namespace Queueware.Dataframework.Core.Services.Entities;

public partial class EntityService<TId, TEntity>
{
    /// <inheritdoc />
    public Task<int> CountAsync(CancellationToken cancellationToken) => repository.CountAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
    {
        return expression != null! ? await repository.CountAsync(expression, cancellationToken) : 0;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken)
    {
        return expression != null! ? (await repository.FindAsync(expression, cancellationToken)).ToList() : [];
    }

    /// <inheritdoc />
    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken)
    {
        return expression != null! ? await repository.FirstOrDefaultAsync(expression, cancellationToken) : default;
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken)
    {
        return id != null ? await repository.GetByIdAsync(id, cancellationToken) : default;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAsync(IEnumerable<TId> ids, CancellationToken cancellationToken)
    {
        var enumeratedIds = ids.Where(id => id != null!).ToList();
        return enumeratedIds.Count > 0
            ? (await repository.GetByIdAsync(enumeratedIds, cancellationToken)).ToList()
            : [];
    }
}