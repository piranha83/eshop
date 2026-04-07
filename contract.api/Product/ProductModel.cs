namespace Contract.Api.Product;

/// <summary>
/// Продукт.
/// </summary>
public record ProductModel
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Цена.
    /// </summary>
    public decimal Price { get; set; }
}