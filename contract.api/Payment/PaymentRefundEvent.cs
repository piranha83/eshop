namespace Contract.Api.Payment;

/// <summary>
/// Команда: оплата не прошла, вернуть деньги.
/// </summary>
public interface PaymentRefundEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; }

    /// <summary>
    /// Вернуть.
    /// </summary>
    decimal Amount { get; }
}