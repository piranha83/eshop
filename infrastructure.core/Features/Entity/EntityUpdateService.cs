using AutoMapper;
using FluentValidation;
using Infrastructure.Core.Abstractions;
using Infrastructure.Core.Extensions;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Features.Entity;

///<inheritdoc/>
internal class EntityUpdateService<TContext, TEntity, TKey, TEntityRequest>(
    TContext context,
    IMapper mapper,
    IValidator<TEntityRequest> validator,
    IOutputCacheStore cacheStore)
    : IEntityUpdateService<TContext, TEntity, TKey, TEntityRequest>
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : struct
    where TEntityRequest : class, new()
{
    ///<inheritdoc/>
    public async Task<TKey> Add(TEntityRequest model, CancellationToken ct)
    {
        await validator.Validate<TEntityRequest>(model);
        var entity = mapper.Map<TEntity>(model);

        using (var transaction = await context.Database.BeginTransactionAsync(ct))
        {
            try
            {
                await context.AddAsync(entity, ct);
                await context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                await cacheStore.EvictByTagAsync(Consts.Cache.FilterPolicy, ct);
            }
            catch
            {
                await transaction.DisposeAsync();
                throw;
            }
        }

        return entity.Id;
    }

    ///<inheritdoc/>
    public async Task<bool> Update(TKey id, TEntityRequest model, CancellationToken ct)
    {
        var removed = false;

        await validator.Validate<TEntityRequest>(model);

        using (var transaction = await context.Database.BeginTransactionAsync(ct))
        {
            try
            {
                if (await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id), ct) is TEntity entity)
                {
                    mapper.Map(model, entity);
                    context.Update(entity);
                    await context.SaveChangesAsync(ct);
                    await cacheStore.EvictByTagAsync(Consts.Cache.FilterPolicy, ct);
                    removed = true;
                }

                await transaction.CommitAsync(ct);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        return removed;
    }
}