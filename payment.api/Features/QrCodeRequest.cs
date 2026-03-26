using Contract.Api.Payment;

namespace Payment.Api.Features;

/// <summary>
/// Регистрация QR-кода.
/// </summary>
internal class QrCodeRequest
{
    /// <summary>
    /// Описание.
    /// </summary>
    public string Description { get; init; } = default!;

    /// <summary>
    /// Итог.
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// Тип валюты.
    /// </summary>
    public CurrencyType Currency { get; init; }

    /// <summary>
    /// ТТЛ в минутах.
    /// </summary>
    public int TtlMinutes { get; set; }
}