namespace Contract.Api.Payment;

/// <summary>
/// Событие: оплата начата.
/// </summary>
public interface PaymentReceivedEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; }

    /// <summary>
    /// Уникальный идентификатор оплаты.
    /// </summary>
    Guid PaymentId { get; }
}