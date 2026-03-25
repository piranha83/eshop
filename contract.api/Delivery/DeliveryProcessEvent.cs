using Contract.Api.Cart;

namespace Contract.Api.Delivery;

/// <summary>
/// Команда: передать заказ в доставку.
/// </summary>
public interface DeliveryProcessEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; }

    /// <summary>
    /// Тип доставки.
    /// </summary>
    DeliveryType Type { get; }

    /// <summary>
    /// Адресс доставки.
    /// </summary>
    string Address { get; }

    /// <summary>
    /// Покупки.
    /// </summary>
    IReadOnlyCollection<CartItemModel> CartItems { get; }
}