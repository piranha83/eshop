using Infrastructure.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Features.Entity;

/// <summary>
/// Обновить ервис.
/// </summary>
public interface IEntityReadService<TContext, TEntity, TKey, TEntityResponse>
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : struct
    where TEntityResponse : class
{
    /// <summary>
    /// Извлечение и просмотр существующих данных.
    /// </summary>
    /// <param name="searchCriteria">Фильтр.</param>
    /// <param name="ct">Маркер отмены.</param>
    /// <returns>Данные.</returns>
    Task<List<TEntityResponse>> Find(SearchCriteria searchCriteria, CancellationToken ct);

    /// <summary>
    /// Извлечение и просмотр существующих данных.
    /// </summary>
    /// <param name="id">Уникальный идентификатор.</param>
    /// <param name="ct">Маркер отмены.</param>
    /// <returns>Данные.</returns>
    Task<TEntityResponse?> Find(TKey id, CancellationToken ct);
}