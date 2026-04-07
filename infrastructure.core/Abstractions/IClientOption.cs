namespace Infrastructure.Core.Abstractions;

/// <summary>
/// Настройки клиента.
/// </summary>
public interface IClientOption
{
    /// <summary>
    /// Сервис.
    /// </summary>
    string Url { get; }

    /// <summary>
    /// Токен(sandbox).
    /// </summary>
    string Token { get; }
}