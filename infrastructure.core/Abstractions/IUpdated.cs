namespace Infrastructure.Core.Abstractions;

/// <summary>
/// Представляет базовую сущность в системе.
/// </summary>
public interface IUpdated
{
    /// <summary>
    /// Имя.
    /// </summary>
    Guid? Updated { get; set; }

    /// <summary>
    /// Дата.
    /// </summary>
    DateTimeOffset? UpdatedDate { get; set; }
}