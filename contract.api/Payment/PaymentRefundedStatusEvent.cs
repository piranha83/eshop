namespace Contract.Api.Payment;

/// <summary>
/// Событие: статус возврата.
/// </summary>
public interface PaymentRefundedStatusEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; }
}