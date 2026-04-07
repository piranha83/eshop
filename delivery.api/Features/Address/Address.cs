namespace Delivery.Api.Features.Address;

/// <summary>
/// Адрес доставки.
/// </summary>
public class Address
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public long AddressId { get; init; }

    /// <summary>
    /// Адресс доставки форматированый в строку.
    /// </summary>
    public string Formated { get; init; } = default!;
}