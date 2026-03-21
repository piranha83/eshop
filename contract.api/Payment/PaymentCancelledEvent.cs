namespace Contract.Api.Payment;

/// <summary>
/// Событие: оплата не прошла.
/// </summary>
public interface PaymentCancelledEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; }

    /// <summary>
    /// Причина.
    /// </summary>
    string Message { get; }
}