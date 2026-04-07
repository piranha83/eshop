namespace Delivery.Api.DatabaseContext.Models;

using Delivery.Api.Features.DeliveryStatus;
using Infrastructure.Core.Abstractions;

///<inheritdoc/>
public class DeliveryModel : IEntity<Guid>, IUpdated
{
    /// <summary>
    /// Уникальный идентификатор доставки.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Уникальный идентификатор клиента.
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Уникальный идентификатор Ya доставки.
    /// </summary>
    public string DeliveryOfferId { get; set; } = default!;

    /// <summary>
    /// Уникальный идентификатор адрес доставки.
    /// </summary>
    public long AddressId { get; set; }

    /// <summary>
    /// Статус доставки.
    /// </summary>
    public DeliveryStatusType Status { get; set; }

    /// <summary>
    /// Кто редактировал.
    /// </summary>
    public Guid? Updated { get; set; }

    /// <summary>
    /// Дата редактирования.
    /// </summary>
    public DateTimeOffset? UpdatedDate { get; set; }
}