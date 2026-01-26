using Infrastructure.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Abstractions;

/// <summary>
/// Общий интерфейс репозитория для сущности типа TEntity.
/// </summary>
/// <typeparam name="TContext">Тип контекста.</typeparam>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
/// <typeparam name="TKey">Тип идентификатора сущности.</typeparam>
public interface IRepository<TContext, TEntity, TKey>
where TContext: DbContext
where TEntity: IEntity<TKey>
where TKey: struct
{
    /// <summary>
    /// Асинхронно получает все записи указанного типа из хранилища.
    /// </summary>
    /// <param name="searchCriteria">Фильтр.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Коллекция всех сущностей.</returns>
    Task<List<TEntity>> Get(SearchCriteria searchCriteria, CancellationToken cancellationToken);

    /// <summary>
    /// Асинхронно получает количество эллементов указанного типа из хранилища.
    /// </summary>
    /// <param name="searchCriteria">Фильтр.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Количество эллементов.</returns>
    Task<long> Total(SearchCriteria searchCriteria, CancellationToken cancellationToken);

    /// <summary>
    /// Асинхронно получает сущность по ее идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Найденная сущность или null, если не найдена.</returns>
    Task<TEntity?> Get(TKey id, CancellationToken cancellationToken);

    /// <summary>
    /// Асинхронно добавляет новую сущность в хранилище.
    /// </summary>
    /// <param name="entity">Сущность для добавления.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Идентификатор сущности.</returns>
    Task<TKey> Add(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Асинхронно обновляет существующую сущность в хранилище.
    /// </summary>
    /// <param name="entity">Сущность для обновления.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Задача, представляющая асинх operações.</returns>
    Task Update(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Асинхронно удаляет сущность по ее идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности для удаления.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Задача, представляющая асинх operações.</returns>
    Task Delete(TKey id, CancellationToken cancellationToken);

    /// <summary>
    /// Асинхронно сохраняет все изменения в хранилище.
    /// </summary>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Задача, представляющая асинх operações.</returns>
    Task SaveChanges(CancellationToken cancellationToken);
}