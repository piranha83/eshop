namespace Contract.Api.Delivery;

/// <summary>
/// Команда: передать заказ в доставку.
/// </summary>
public interface DeliveryProcessEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; set; }

    /// <summary>
    /// Уникальный идентификатор клиента.
    /// </summary>
    Guid ClientId { get; set; }

    /// <summary>
    /// Тип доставки.
    /// </summary>
    DeliveryType Type { get; set; }

    /// <summary>
    /// Адресс доставки.
    /// </summary>
    string Address { get; set; }
}