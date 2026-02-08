using Infrastructure.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Features.Entity;

/// <summary>
/// Сервис обновления.
/// </summary>
public interface IEntityUpdateService<TContext, TEntity, TKey, TEntityRequest>
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : struct
    where TEntityRequest : class, new()
{
    /// <summary>
    /// Добавить сущность.
    /// </summary>
    /// <param name="model">Сущность для добавления.</param>
    /// <param name="ct">Маркер отмены.</param>
    /// <returns>Идентификатор сущности.</returns>
    Task<TKey> Add(TEntityRequest model, CancellationToken ct);

    /// <summary>
    /// Обновить сущность.
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности.</param>
    /// <param name="model">Сущность для обновления.</param>
    /// <param name="ct">Маркер отмены.</param>
    /// <returns>Идентификатор сущности.</returns>
    Task Update(TKey id, TEntityRequest model, CancellationToken ct);
}