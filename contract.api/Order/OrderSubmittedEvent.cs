using Contract.Api.Cart;
using Contract.Api.Delivery;

namespace Contract.Api.Order;

/// <summary>
/// Событие: Заказ создан.
/// </summary>
public interface OrderSubmittedEvent
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

    /// <summary>
    /// Покупки.
    /// </summary>
    IReadOnlyCollection<CartItemModel> CartItems { get; set; }

    /// <summary>
    /// Итого.
    /// </summary>
    decimal Amount { get; set; }
}