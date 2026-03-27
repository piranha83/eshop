namespace Infrastructure.Core;

/// <summary>
/// Настройки.
/// </summary>
public sealed record RabbitOptions
{
    /// <summary>
    /// Сервис.
    /// </summary>
    public string Host { get; set; } = default!;

    /// <summary>
    /// Логин.
    /// </summary>
    public string User { get; set; } = default!;

    /// <summary>
    /// Пароль.
    /// </summary>
    public string Password { get; set; } = default!;

    /// <summary>
    /// Директория.
    /// </summary>
    public string Vhost { get; set; } = default!;

    /// <summary>
    /// Порт.
    /// </summary>
    public int Port { get; set; } = default!;

    /// <summary>
    /// SSl
    /// </summary>
    public bool UseSsl { get; set; } = default!;
}