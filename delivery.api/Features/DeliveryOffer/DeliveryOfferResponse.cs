using Delivery.Api.Features.DeliveryStatus;

namespace Delivery.Api.Features.DeliveryOffer;

/// <summary>
/// Заказ доставки.
/// </summary>
public sealed record DeliveryOfferResponse
{
    /// <summary>
    /// Уникальный идентификатор доставки заказа.
    /// </summary>
    public string DeliveryOfferId { get; init; } = default!;

    /// <summary>
    /// Цена.
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// Статус доставки.
    /// </summary>
    public DeliveryStatusType Status { get; init; }
}