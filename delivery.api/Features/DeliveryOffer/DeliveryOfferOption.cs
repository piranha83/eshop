using Infrastructure.Core.Abstractions;

namespace Delivery.Api.Features.DeliveryOffer;

/// <summary>
/// Настройки.
/// </summary>
internal sealed class DeliveryOfferOption : IClientOption
{
    /// <summary>
    /// Сервис.
    /// </summary>
    public string Url { get; set; } = default!;

    /// <summary>
    /// Токен(sandbox).
    /// </summary>
    public string Token { get; set; } = default!;

    /// <summary>
    /// Откуда забрать заказ(sandbox).
    /// </summary>
    public string? SourceStationId { get; set; }
    
    /// <summary>
    /// Куда доставить заказ(sandbox).
    /// </summary>
    public string? DestanationStationId { get; set; }
}