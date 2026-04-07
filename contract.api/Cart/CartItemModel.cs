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
    public ProductModel Product { get; set; } = default!;

    /// <summary>
    /// Количество.
    /// </summary>
    public int Count { get; set; }
}