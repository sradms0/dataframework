using System.Linq.Expressions;

namespace Queueware.Dataframework.Core.Services.Entities;

public partial class EntityService<TId, TEntity>
{
    /// <inheritdoc />
    public Task<bool> SaveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return SaveAsync([entity], cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> SaveAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        var areSaved = false;
        var enumeratedEntities = entities.Where(entity => entity != null!).ToList();
        if (enumeratedEntities.Count > 0)
        {
            areSaved = (await InsertAsync(enumeratedEntities, cancellationToken) ?? true) && 
                       (await UpdateAsync(enumeratedEntities, cancellationToken) ?? true);
        }

        return areSaved;
    }

    /// <inheritdoc />
    public async Task<bool> SaveAsync<TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value,
        CancellationToken cancellationToken)
    {
        var isEntityValid = entity != null! && entity.Id != null && !entity.Id.Equals(default(TId));

        return isEntityValid && field != null!
                             && await repository.UpdateAsync(entity!, field, value, cancellationToken);
    }

    private async Task<bool?> InsertAsync(List<TEntity> entities, CancellationToken cancellationToken)
    {
        bool? areInserted = null;
        
        var newEntities = entities
            .Where(entity => entity.Id == null || entity.Id.Equals(default(TId)))
            .ToList();
    
        if (newEntities.Count > 0)
        {
            areInserted = await repository.InsertAsync(newEntities, cancellationToken);
        }

        return areInserted;
    }

    private async Task<bool?> UpdateAsync(List<TEntity> entities, CancellationToken cancellationToken)
    {
        bool? areUpdated = null;
        
        var updatedEntities = entities
            .Where(entity => entity.Id != null && !entity.Id.Equals(default(TId)))
            .ToList();
        
        if (updatedEntities.Count > 0)
        {
            areUpdated = await repository.UpdateAsync(updatedEntities, cancellationToken);
        }

        return areUpdated;
    }
}