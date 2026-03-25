namespace Contract.Api.Delivery;

/// <summary>
/// Событие: Доставка заказа отменена.
/// </summary>
public interface DeliveryCancelledEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; }
}