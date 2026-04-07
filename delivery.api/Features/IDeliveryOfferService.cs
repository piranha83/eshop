using Delivery.Api.Features.DeliveryOffer;

namespace Delivery.Api.Features;

/// <summary>
/// Сервис ya доставки заказов.
/// </summary>
public interface IDeliveryOfferService
{
    /// <summary>
    /// Создание/подтверждение заявки доставки.
    /// </summary>
    /// <param name="request">Данные заявки.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Заказ доставки.</returns>
    public Task<DeliveryOfferCreateResponse> Create(DeliveryOfferCreateRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Отмена заявки доставки.
    /// </summary>
    /// <param name="deliveryOfferId">Уникальный идентификатор доставки заказа.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>True - отменяем задачу.</returns>
    public Task<bool> Cancel(string deliveryOfferId, CancellationToken cancellationToken);

    /// <summary>
    /// Получение информации о заявки доставки.
    /// </summary>
    /// <param name="deliveryOfferId">Уникальный идентификатор доставки заказа.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>True - отменяем задачу.</returns>
    public Task<DeliveryOfferResponse> Details(string deliveryOfferId, CancellationToken cancellationToken);
}