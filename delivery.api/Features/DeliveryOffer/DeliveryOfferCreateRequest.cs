using Contract.Api.Cart;

namespace Delivery.Api.Features.DeliveryOffer;

/// <summary>
/// Создание/подтверждение доставки заказа.
/// </summary>
public sealed record DeliveryOfferCreateRequest
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    public Guid OrderId { get; init; }

    /// <summary>
    /// Адресс доставки.
    /// </summary>
    public string Address { get; init; } = default!;

    /// <summary>
    /// Покупки.
    /// </summary>
    public IReadOnlyList<CartItemModel> CartItems { get; init; } = default!;

    /// <summary>
    /// Телефон.
    /// </summary>
    public string Phone { get; init; } = default!;

    /// <summary>
    /// Имя.
    /// </summary>
    public string FirstName { get; init; } = default!;
}