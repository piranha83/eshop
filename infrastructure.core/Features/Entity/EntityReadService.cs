using AutoMapper;
using Infrastructure.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Extensions;

///<inheritdoc/>
internal class EntityReadService<TContext, TEntity, TKey, TEntityResponse>(
    IHttpContextAccessor contextAccessor,
    TContext context,
    IMapper mapper) : IEntityReadService<TContext, TEntity, TKey, TEntityResponse>
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : struct
    where TEntityResponse : class
{
    ///<inheritdoc/>
    public async Task<List<TEntityResponse>> Find(SearchCriteria searchCriteria, CancellationToken ct)
    {
        searchCriteria.SetTerm(contextAccessor);
        var entity = context.Set<TEntity>().AsNoTracking();
        var data = await entity.Search<TEntity, TKey>(searchCriteria).ToListAsync(ct);
        var total = await entity.Search<TEntity, TKey>(searchCriteria).CountAsync(ct);

        contextAccessor.HttpContext!.Response.Headers.Append("x-total-count", total.ToString());

        return mapper.Map<List<TEntityResponse>>(data);
    }

    ///<inheritdoc/>
    public async Task<TEntityResponse?> Find(TKey id, CancellationToken ct)
    {
        var model = await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

        return model != null ? mapper.Map<TEntityResponse>(model) : null;
    }
}