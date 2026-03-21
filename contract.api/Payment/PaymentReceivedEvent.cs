namespace Contract.Api.Payment;

/// <summary>
/// Событие: оплата прошла успешно.
/// </summary>
public interface PaymentReceivedEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; set; }

    /// <summary>
    /// Уникальный идентификатор оплаты.
    /// </summary>
    Guid PaymentId { get; set; }
}