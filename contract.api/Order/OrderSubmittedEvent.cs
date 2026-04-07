using Contract.Api.Cart;
using Contract.Api.Delivery;
using Contract.Api.Payment;

namespace Contract.Api.Order;

/// <summary>
/// Событие: Заказ создан.
/// </summary>
public interface OrderSubmittedEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; }

    /// <summary>
    /// Уникальный идентификатор клиента.
    /// </summary>
    Guid ClientId { get; }

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
    IReadOnlyList<CartItemModel> CartItems { get; }

    /// <summary>
    /// Итого.
    /// </summary>
    decimal Amount { get; }

    /// <summary>
    /// Тип валюты.
    /// </summary>
    CurrencyType CurrencyType { get; }

    /// <summary>
    /// Тип оплаты.
    /// </summary>
    PaymentType PaymentType { get; }
}