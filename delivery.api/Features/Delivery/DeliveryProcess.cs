using Contract.Api.Cart;
using Contract.Api.Delivery;

namespace Delivery.Api.Features.Delivery;

/// <summary>
/// Команда: передать заказ в доставку.
/// </summary>
public record DeliveryProcess
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Уникальный идентификатор клиента.
    /// </summary>
    public Guid ClientId { get; set; }

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
    public IReadOnlyList<CartItemModel> CartItems { get; set; } = default!;
}