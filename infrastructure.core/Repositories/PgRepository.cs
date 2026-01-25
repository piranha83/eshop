using Infrastructure.Core.Abstractions;
using Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Repositories;

///<inheritdoc/>
internal class PgRepository<TContext, TEntity, TKey> : IRepository<TContext, TEntity, TKey>
where TContext: DbContext
where TEntity : class, IEntity<TKey>
where TKey: struct
{
    private readonly TContext _context;
    private readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// Коструктор.
    /// </summary>
    /// <param name="context">Контекст.</param>
    public PgRepository(TContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    ///<inheritdoc/>
    public async Task<TKey> Add(TEntity entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    ///<inheritdoc/>
    public async Task<List<TEntity>> Get(SearchCriteria searchCriteria, CancellationToken cancellationToken)
    {
        IQueryable<TEntity> data = _dbSet;
        if (searchCriteria.Include != null)
            data = data.Include(searchCriteria.Include.Trim());
        if (searchCriteria.Term != null)
            data = data.Where(searchCriteria.Term);
        if (searchCriteria.SortField != null && searchCriteria.SortDirection != null)
            data = data.OrderBy(searchCriteria.SortField, searchCriteria.SortDirection.Value);
        if (searchCriteria.Skip != null && searchCriteria.Take != null)
            data = data.Skip((searchCriteria.Skip.Value - 1) * searchCriteria.Take.Value);
        if (searchCriteria.Take != null)
            data = data.Take(searchCriteria.Take.Value);

        return await data.ToListAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<long> Total(SearchCriteria searchCriteria, CancellationToken cancellationToken)
    {
        IQueryable<TEntity> data = _dbSet;
        if (searchCriteria.Include != null)
            data = data.Include(searchCriteria.Include.Trim());
        if (searchCriteria.Term != null)
            data = data.Where(searchCriteria.Term);
        if (searchCriteria.SortField != null && searchCriteria.SortDirection != null)
            data = data.OrderBy(searchCriteria.SortField, searchCriteria.SortDirection.Value);

        return await data.CountAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public Task<TEntity?> Get(TKey id, CancellationToken cancellationToken) =>
        _dbSet.FirstOrDefaultAsync(x => Equals(x.Id, id), cancellationToken);

    ///<inheritdoc/>
    public Task Update(TEntity entity, CancellationToken cancellationToken) =>
        _dbSet.Where(x => Equals(x.Id, entity.Id))
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Id, entity.Id));

    ///<inheritdoc/>
    public Task Delete(TKey id, CancellationToken cancellationToken) =>
        _dbSet.Where(x => Equals(x.Id, id))
            .ExecuteDeleteAsync(cancellationToken);

    ///<inheritdoc/>
    public Task SaveChanges(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);
}