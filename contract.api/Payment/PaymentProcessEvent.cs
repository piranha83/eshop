namespace Contract.Api.Payment;

/// <summary>
/// Событие: оплата заказа.
/// </summary>
public interface PaymentProcessEvent
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    Guid OrderId { get; set; }

    /// <summary>
    /// Уникальный идентификатор клиента.
    /// </summary>
    Guid ClientId { get; set; }

    /// <summary>
    /// Итог.
    /// </summary>
    decimal Amount { get; set; }

    /// <summary>
    /// Тип валюты.
    /// </summary>
    CurrencyType Currency { get; set; }

    /// <summary>
    /// Тип оплаты.
    /// </summary>
    PaymentType Type { get; set; }
}