namespace Contract.Api.Delivery;

/// <summary>
/// Событие: Доставка согласована.
/// </summary>
public interface DeliveryAcceptedEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; }
}