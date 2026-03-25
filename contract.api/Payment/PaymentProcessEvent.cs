namespace Contract.Api.Payment;

/// <summary>
/// Событие: оплата заказа.
/// </summary>
public interface PaymentProcessEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; }

    /// <summary>
    /// Уникальный идентификатор клиента.
    /// </summary>
    Guid ClientId { get; }

    /// <summary>
    /// Итог.
    /// </summary>
    decimal Amount { get; }

    /// <summary>
    /// Тип валюты.
    /// </summary>
    CurrencyType CurrencyType { get; }

    /// <summary>
    /// Тип оплаты.
    /// </summary>
    PaymentType PaymentType { get; }
}