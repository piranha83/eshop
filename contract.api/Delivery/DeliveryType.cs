namespace Contract.Api.Delivery;

/// <summary>
/// Тип доставки.
/// </summary>
public enum DeliveryType
{
    /// <summary>
    /// Доставка по адрессу клиента.
    /// </summary>
    Athome,

    /// <summary>
    /// Доставка в магазин или точку выдачи товаров.
    /// </summary>
    Atshop,
}