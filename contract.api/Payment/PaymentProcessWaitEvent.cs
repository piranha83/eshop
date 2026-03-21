namespace Contract.Api.Payment;

/// <summary>
/// Событие: ждем завершения оплаты заказа.
/// </summary>
public interface PaymentProcessWaitEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; set; }

    /// <summary>
    /// Уникальный идентификатор qr кода.
    /// </summary>
    string QrCodeId { get; set; }
}