namespace Contract.Api.Delivery;

/// <summary>
/// Команда: заказ потвержден успешно.
/// </summary>
public interface OrderConfirmedEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; }
}