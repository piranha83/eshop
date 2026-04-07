namespace Delivery.Api.Features.DeliveryOffer;

/// <summary>
/// Заказ доставки.
/// </summary>
public sealed record DeliveryOfferCreateResponse
{
    /// <summary>
    /// Уникальный идентификатор доставки заказа.
    /// </summary>
    public string DeliveryOfferId { get; init; } = default!;

    /// <summary>
    /// Уникальный идентификатор адресса доставки.
    /// </summary>
    public long AddressId { get; init; } = default!;
}