using AutoMapper;
using Infrastructure.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Extensions;

///<inheritdoc/>
internal class EntityUpdateService<TContext, TEntity, TKey, TEntityRequest>(
    TContext context,
    IMapper mapper) : IEntityUpdateService<TContext, TEntity, TKey, TEntityRequest>
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : struct
    where TEntityRequest : class, new()
{
    ///<inheritdoc/>
    public async Task<TKey> Add(TEntityRequest model, CancellationToken ct)
    {
        var entity = mapper.Map<TEntity>(model);
        await context.AddAsync(entity, ct);
        await context.SaveChangesAsync(ct);

        return entity.Id;
    }

    ///<inheritdoc/>
    public async Task Update(TKey id, TEntityRequest model, CancellationToken ct)
    {
        var entity = await context.Set<TEntity>().FirstAsync(x => x.Id.Equals(id), ct);
        mapper.Map(model, entity);

        await context.SaveChangesAsync(ct);
    }
/*
    ///<inheritdoc/>
    public async Task Patch(TKey id, JsonPatchDocument<TEntityRequest> patch, CancellationToken ct)
    {
        TEntityRequest model = new();
        patch.ApplyTo(model);

        var entity = await context.Set<TEntity>().FirstAsync(x => x.Id.Equals(id), ct);
        mapper.Map(entity, model);
        await context.SaveChangesAsync(ct);
    }
*/
}