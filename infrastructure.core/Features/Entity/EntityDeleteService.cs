using Infrastructure.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Features.Entity;

///<inheritdoc/>
internal class EntityDeleteService<TContext, TEntity, TKey>(
    TContext context) : IEntityDeleteService<TContext, TEntity, TKey>
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : struct
{
    ///<inheritdoc/>
    public async Task<bool> Delete(TKey id, CancellationToken ct)
    {
        var removed = false;

        using (var transaction = await context.Database.BeginTransactionAsync(ct))
        {
            try
            {
                var dbSet = context.Set<TEntity>();
                if (await dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id), ct) is TEntity entity)
                {
                    dbSet.Remove(entity);
                    await context.SaveChangesAsync(ct);
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