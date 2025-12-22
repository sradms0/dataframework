using Dataframework.Abstractions.Primitives;

namespace Dataframework.Infrastructure.Primitives;

public abstract class EntityBase<TId> : IId<TId>
{
    public TId Id { get; set; } = default!;
}