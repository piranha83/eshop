using System.Text.Json.Serialization;
using Contract.Api.Cart;
using Contract.Api.Delivery;
using Contract.Api.Order;
using Contract.Api.Payment;

namespace Order.Api.Features.Order;

/// <summary>
/// Создать заказ.
/// </summary>
public record OrderModel : OrderSubmittedEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    [JsonIgnore]
    public Guid OrderId { get; init; }

    /// <summary>
    /// Уникальный идентификатор клиента.
    /// </summary>
    [JsonIgnore]
    public Guid ClientId { get; init; }

    /// <summary>
    /// Тип доставки.
    /// </summary>
    public DeliveryType Type { get; set; }

    /// <summary>
    /// Адресс доставки.
    /// </summary>
    public string Address { get; set; } = default!;

    /// <summary>
    /// Покупки.
    /// </summary>
    public IReadOnlyCollection<CartItemModel> CartItems { get; set; } = default!;

    /// <summary>
    /// Итого.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Тип валюты.
    /// </summary>
    public CurrencyType CurrencyType { get; set; }

    /// <summary>
    /// Тип оплаты.
    /// </summary>
    public PaymentType PaymentType { get; set; }
}