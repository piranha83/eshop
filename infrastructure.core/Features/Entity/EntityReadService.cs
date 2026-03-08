using AutoMapper;
using FluentValidation;
using Infrastructure.Core.Abstractions;
using Infrastructure.Core.Extensions;
using Infrastructure.Core.Features.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Features.Entity;

///<inheritdoc/>
internal class EntityReadService<TContext, TEntity, TKey, TEntityResponse>(
    IContextAccessor accessor,
    TContext context,
    IMapper mapper,
    IValidator<SearchCriteria> validator)
    : IEntityReadService<TContext, TEntity, TKey, TEntityResponse>
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : struct
    where TEntityResponse : class
{
    ///<inheritdoc/>
    public async Task<List<TEntityResponse>> Find(SearchCriteria searchCriteria, CancellationToken ct)
    {
        await validator.Validate<SearchCriteria>(searchCriteria);
        accessor.SetTerm(searchCriteria);

        var entity = context.Set<TEntity>().AsNoTracking();
        var data = await entity.Search<TEntity, TKey>(searchCriteria).ToListAsync(ct);
        var total = await entity.Search<TEntity, TKey>(searchCriteria).CountAsync(ct);

        accessor.SetTotal(total);
        return mapper.Map<List<TEntityResponse>>(data);
    }

    ///<inheritdoc/>
    public async Task<TEntityResponse?> Find(TKey id, CancellationToken ct)
    {
        var entity = context.Set<TEntity>().AsNoTracking();
        return await entity.FirstOrDefaultAsync(x => x.Id.Equals(id)) is TEntity model ? mapper.Map<TEntityResponse>(model) : null;
    }
}