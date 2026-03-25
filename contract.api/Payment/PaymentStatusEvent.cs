namespace Contract.Api.Payment;

/// <summary>
/// Событие: получить статус оплаты заказа.
/// </summary>
public interface PaymentStatusEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; }
}