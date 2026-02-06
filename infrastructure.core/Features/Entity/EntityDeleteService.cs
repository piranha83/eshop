using Infrastructure.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Extensions;

///<inheritdoc/>
internal class EntityDeleteService<TContext, TEntity, TKey>(
    TContext context) : IEntityDeleteService<TContext, TEntity, TKey>
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : struct
{
    ///<inheritdoc/>
    public async Task Delete(TKey id, CancellationToken ct)
    {
        await context.Set<TEntity>().Where(x => x.Id.Equals(id)).ExecuteDeleteAsync(ct);
    }
}