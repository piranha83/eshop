using Payment.Api.Features.PaymentStatus;

namespace Payment.Api.Features;

/// <summary>
/// Ответ QR-кода.
/// </summary>
internal class QrCodeResponse
{
    /// <summary>
    /// Уникальный идентификатор оплаты.
    /// </summary>
    public string Id { get; init; } = default!;

    /// <summary>
    /// Ссылка.
    /// </summary>
    public string Payload { get; init; } = default!;

    /// <summary>
    /// Статус оплаты.
    /// </summary>
    public PaymentStatusType Status { get; init; }
}