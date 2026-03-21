using Contract.Api.Product;

namespace Contract.Api.Cart;

/// <summary>
/// Покупка.
/// </summary>
public record CartItemModel
{
    /// <summary>
    /// Продукт.
    /// </summary>
    ProductModel Product { get; set; } = default!;

    /// <summary>
    /// Количество.
    /// </summary>
    int Count { get; set; }
}