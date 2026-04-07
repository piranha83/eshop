namespace Delivery.Api.Features.DeliveryStatus;

/// <summary>
/// Статус доставки.
/// </summary>
public enum DeliveryStatusType
{
    /// <summary>
    /// Заказ принят в доставку.
    /// </summary>
    Accepted,

    /// <summary>
    /// В пути, заказ направляется клиенту.
    /// </summary>
    InTransit,

    /// <summary>
    /// Заказ доставлен.
    /// </summary>
    Delivered,

    /// <summary>
    /// Заказ возвращаен.
    /// </summary>
    Returned,

    /// <summary>
    /// Заказ не доставлен.
    /// </summary>
    Canceled,
}
