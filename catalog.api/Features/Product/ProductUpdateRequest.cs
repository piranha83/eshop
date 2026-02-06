using System.ComponentModel;

namespace Catalog.Api.Features.Product;

/// <summary>
/// Обновить.
/// </summary>
[Description("Обновить")]
public class ProductUpdateRequest
{
    /// <summary>
    /// Имя.
    /// </summary>
    [Description("Имя")]
    public string? Name { get; set; }

    /// <summary>
    /// Описание.
    /// </summary>
    [Description("Описание")]
    public string? Description { get; set; }

    /// <summary>
    /// Цена.
    /// </summary>
    [Description("Цена")]
    public decimal? Price { get; set; }

    /// <summary>
    /// В наличии.
    /// </summary>
    [Description("В наличии")]
    public bool? InStock { get; set; }

    /// <summary>
    /// Скидка.
    /// </summary>
    [Description("Скидка")]
    public decimal? Discount { get; set; }

    /// <summary>
    /// Картинка.
    /// </summary>
    [Description("Картинка")]
    public string? Img { get; set; }

    /// <summary>
    /// Рейтинг.
    /// </summary>
    [Description("Рейтинг")]
    public int? Rate { get; set; }

    /// <summary>
    /// Идентигфикатор каталога.
    /// </summary>
    [Description("Идентигфикатор каталога")]
    public long? CatalogId { get; set; }
}