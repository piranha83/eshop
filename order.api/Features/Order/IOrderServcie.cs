namespace Order.Api.Features.Order;

/// <summary>
/// Сервис заказа.
/// </summary>
public interface IOrderServcie
{
    /// <summary>
    /// Создать заказ.
    /// </summary>
    /// <param name="reqest">Заказ.</param>
    /// <returns>Уникальный идентификатор заказа.</returns>
    Task<Guid> Create(OrderModel reqest, CancellationToken cancellationToken);
}