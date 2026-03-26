using Payment.Api.Features.PaymentStatus;

namespace Payment.Api.Features.PaymentStatusSet;

/// <summary>
/// Событие: статус оплаты заказа изменен.
/// </summary>
public interface PaymentStatusSetEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; }

    /// <summary>
    /// Статус.
    /// </summary>
    PaymentStatusType PaymentStatus { get; }
}