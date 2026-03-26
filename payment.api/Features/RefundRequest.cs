using Contract.Api.Payment;

namespace Payment.Api.Features;

/// <summary>
/// Возврат.
/// </summary>
internal class RefundRequest
{
    /// <summary>
    /// Уникальный идентификатор qr кода.
    /// </summary>
    public string QrCodeId { get; init; } = default!;

    /// <summary>
    /// Описание.
    /// </summary>
    public string Description { get; init; } = default!;

    /// <summary>
    /// Сумма.
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// Тип валюты.
    /// </summary>
    public CurrencyType Currency { get; init; }

    /// <summary>
    /// Идентификатор транзакции.
    /// </summary>
    public string? TransactionId { get; init; }

    /// <summary>
    /// Идентификатор операции в НСПК.
    /// </summary>
    public string? TrxId { get; init; }
}