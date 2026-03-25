namespace Contract.Api.Delivery;

/// <summary>
/// Событие: Доставка завершена успешно.
/// </summary>
public interface DeliveredEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; }

    /// <summary>
    /// Уникальный идентификатор доставки.
    /// </summary>
    Guid DeliveryId { get; }
}