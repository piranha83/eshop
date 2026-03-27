using Contract.Api.Payment;
using Infrastructure.Core.Abstractions;
using Payment.Api.Features.PaymentStatus;

namespace Payment.Api.DatabaseContext.Models;

///<inheritdoc/>
public class PaymentModel : IEntity<Guid>, IUpdated
{
    /// <summary>
    /// Уникальный идентификатор отплаты.
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
    /// Итого.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Валюта.
    /// </summary>
    public CurrencyType Currency { get; set; }

    /// <summary>
    /// Тип оплаты.
    /// </summary>
    public PaymentType Type { get; set; }

    /// <summary>
    /// Уникальный идентификатор qr кода.
    /// </summary>
    public string? QrCodeId { get; set; }

    /// <summary>
    /// Ссылка qr кода.
    /// </summary>
    public string? QrCodePayload { get; set; }

    /// <summary>
    /// Статус оплаты.
    /// </summary>
    public PaymentStatusType Status { get; set; }

    /// <summary>
    /// Кто редактировал.
    /// </summary>
    public Guid? Updated { get; set; }

    /// <summary>
    /// Дата редактирования.
    /// </summary>
    public DateTimeOffset? UpdatedDate { get; set; }
}