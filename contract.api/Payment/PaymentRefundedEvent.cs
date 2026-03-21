namespace Contract.Api.Payment;

/// <summary>
/// Событие: деньги вернули за аннулированный заказ
/// </summary>
public interface PaymentRefundedEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; }
}