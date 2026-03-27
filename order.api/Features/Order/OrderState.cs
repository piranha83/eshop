using MassTransit;

namespace Order.Api.Features.Order;

///<inheritdoc/>
public class OrderState : SagaStateMachineInstance
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    public Guid CorrelationId { get; set; }

    /// <summary>
    /// Статус заказа.
    /// </summary>
    public string State { get; set; } = default!;

    /// <summary>
    /// Уникальный идентификатор клиента.
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>
    /// Уникальный идентификатор оплаты.
    /// </summary>
    public Guid? PaymentId { get; set; }

    /// <summary>
    /// Уникальный идентификатор доставки.
    /// </summary>
    public Guid? DeliveryId { get; set; }

    /// <summary>
    /// Уникальный идентификатор таймера.
    /// </summary>
    public Guid? SheduleId { get; set; }

    /// <summary>
    /// Дата.
    /// </summary>
    public DateTimeOffset CreatedDate { get; set; }
}