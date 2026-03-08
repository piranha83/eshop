namespace Infrastructure.Core.Features.Context;

/// <summary>
/// Представляет контекст в системе.
/// </summary>
public record User
{
    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    public Guid? UserId { get; init; }
}