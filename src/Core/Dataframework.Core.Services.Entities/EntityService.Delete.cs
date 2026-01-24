using Queueware.Dataframework.Abstractions.Primitives;
using Queueware.Dataframework.Abstractions.Repositories;
using Queueware.Dataframework.Abstractions.Services.Entities;

namespace Queueware.Dataframework.Core.Services.Entities;

/// <inheritdoc />
public partial class EntityService<TId, TEntity>(IRepository<TId, TEntity> repository)
    : IEntityService<TId, TEntity> where TEntity : IId<TId>
{
    /// <inheritdoc />
    public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return entity != null! && await repository.DeleteAsync(entity, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        var enumeratedEntities = entities.Where(entity =>  entity != null!).ToList();
        return enumeratedEntities.Count > 0 && await repository.DeleteAsync(enumeratedEntities, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken)
    {
        return id != null! && await repository.DeleteAsync(id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(IEnumerable<TId> ids, CancellationToken cancellationToken)
    {
        var enumeratedIds = ids.Where(id => id != null).ToList();
        return enumeratedIds.Count > 0 && await repository.DeleteAsync(enumeratedIds, cancellationToken); 
    }
}