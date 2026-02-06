using Infrastructure.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Extensions;

/// <summary>
/// Сервис удаления.
/// </summary>
public interface IEntityDeleteService<TContext, TEntity, TKey>
{
    /// <summary>
    /// Удаление записи из базы данных.
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности.</param>
    /// <param name="ct">Маркер отмены.</param>
    /// <returns>Задача.</returns>
    Task Delete(TKey id, CancellationToken ct);
}