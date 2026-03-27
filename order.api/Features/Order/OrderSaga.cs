using Contract.Api.Delivery;
using Contract.Api.Order;
using Contract.Api.Payment;
using Infrastructure.Core;
using MassTransit;

namespace Order.Api.Features.Order;

/// <summary>
/// Обработчик состояний заказа.
/// </summary>
internal class OrderSaga : MassTransitStateMachine<OrderState>
{
    private readonly ILogger<OrderSaga> _logger;

    ///<inheritdoc/>
    public OrderSaga(ILogger<OrderSaga> logger)
    {
        _logger = logger;
        InstanceState(x => x.State);

        Event(() => OrderSubmittedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => PaymentReceivedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => PaymentCancelledEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => PaymentRefundedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => DeliveredEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => DeliveryCancelledEvent, x => x.CorrelateById(context => context.Message.OrderId));

        Schedule(() => PaymentStatusEvent, instance => instance.SheduleId, x =>
        {
            x.Received = config => config.CorrelateById(context => context.Message.OrderId);
            x.Delay = TimeSpan.FromMinutes(Consts.QrCodesTTLInMinutes);
        });

        BeforeEnterAny(binder => binder.Then(context => _logger.LogInformation(context.Message.Name)));
        AfterLeaveAny(binder => binder.Then(context => _logger.LogInformation(context.Message.Name)));

        Initially(
            // Заказ создан -> оплатить.
            When(OrderSubmittedEvent)
                .Then(context =>
                {
                    context.Saga.ClientId = context.Message.ClientId;
                    context.Saga.CreatedDate = DateTime.UtcNow;
                })
                // Планируем проверку cтатуса оплаты.
                .Schedule(PaymentStatusEvent, context => context.Init<PaymentStatusEvent>(new
                {
                    context.Message.OrderId,
                }))
                .PublishAsync(context => context.Init<PaymentProcessEvent>(context.Message))
                .TransitionTo(Submitted));

        During(Submitted,
            // Получить статус оплаты.
            When(PaymentStatusEvent.Received)
                .PublishAsync(context => context.Init<PaymentStatusEvent>(new
                {
                    context.Message.OrderId,
                })),
            // Оплачен -> передать в доставку.
            When(PaymentReceivedEvent)
                .Unschedule(PaymentStatusEvent)
                .Then(context =>
                {
                    context.Saga.PaymentId = context.Message.PaymentId;
                })
                .PublishAsync(context => context.Init<DeliveryProcessEvent>(context.Message))
                .TransitionTo(Paid),
            // Ошибка оплаты -> завершить.
            When(PaymentCancelledEvent)
                .Finalize());

        During(Paid,
            // Доставка прянята -> доставлен.
            When(DeliveredEvent)
                .Then(context =>
                {
                    context.Saga.DeliveryId = context.Message.DeliveryId;
                })
                .TransitionTo(Delivered)
                .Finalize(),
            // Доставка отменена -> возвращаем деньги.
            When(DeliveryCancelledEvent)
                .PublishAsync(context => context.Init<PaymentRefundEvent>(new
                {
                    context.Message.OrderId,
                }))
                .TransitionTo(Refunded));

        During(Refunded,
            // Начинаем возврат денег за аннулированный заказ.
            When(PaymentRefundedEvent)
                .Unschedule(PaymentRefundedStatusEvent)
                .Finalize());

        // Удалить из бд при Finalize().
        //SetCompletedWhenFinalized();
    }

    #region Состояния

    /// <summary>
    /// Создан.
    /// </summary>
    public State Submitted { get; private set; } = default!;

    /// <summary>
    /// Оплачен.
    /// </summary>
    public State Paid { get; private set; } = default!;

    /// <summary>
    /// Возвращен.
    /// </summary>
    public State Refunded { get; private set; } = default!;

    /// <summary>
    /// Заказ доставлен.
    /// </summary>
    public State Delivered { get; private set; } = default!;

    #endregion

    #region События

    /// <summary>
    /// Заказ создан.
    /// </summary>
    public Event<OrderSubmittedEvent> OrderSubmittedEvent { get; private set; } = default!;

    /// <summary>
    /// Оплата прошла успешно.
    /// </summary>
    public Event<PaymentReceivedEvent> PaymentReceivedEvent { get; private set; } = default!;

    /// <summary>
    /// Оплата не прошла.
    /// </summary>
    public Event<PaymentCancelledEvent> PaymentCancelledEvent { get; private set; } = default!;

    /// <summary>
    /// Деньги вернули за аннулированный заказ.
    /// </summary>
    public Event<PaymentRefundedEvent> PaymentRefundedEvent { get; private set; } = default!;

    /// <summary>
    /// Доставка заказа отменена.
    /// </summary>
    public Event<DeliveryCancelledEvent> DeliveryCancelledEvent { get; private set; } = default!;

    /// <summary>
    /// Доставка заказа клиенту.
    /// </summary>
    public Event<DeliveredEvent> DeliveredEvent { get; private set; } = default!;

    /// <summary>
    /// Статус оплаты.
    /// </summary>
    public Schedule<OrderState, PaymentStatusEvent> PaymentStatusEvent { get; private set; } = default!;

    /// <summary>
    /// Статус возврата.
    /// </summary>
    public Schedule<OrderState, PaymentRefundedStatusEvent> PaymentRefundedStatusEvent { get; private set; } = default!;

    #endregion
}
