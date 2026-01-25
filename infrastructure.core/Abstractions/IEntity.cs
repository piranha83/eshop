namespace Infrastructure.Core.Abstractions;

/// <summary>
/// Представляет базовую сущность в системе.
/// <typeparam name="TKey">Тип идентификатора сущности.</typeparam>
/// </summary>
public interface IEntity<TKey>
where TKey: struct
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    TKey Id { get; set; }
}