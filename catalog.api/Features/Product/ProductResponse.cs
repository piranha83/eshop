using System.ComponentModel;
using Catalog.Api.DatabaseContext.Models;

namespace Catalog.Api.Features.Product;

/// <summary>
/// Продукт.
/// </summary>
[Description("Продукт")]
public class ProductResponse
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    [Description("Идентификатор")]
    public long Id { get; set; }

    /// <summary>
    /// Имя.
    /// </summary>
    [Description("Имя")]
    public string Name { get; set; } = default!;

    /// <summary>
    /// Описание.
    /// </summary>
    [Description("Описание")]
    public string? Description { get; set; }

    /// <summary>
    /// Цена.
    /// </summary>
    [Description("Цена")]
    public decimal Price { get; set; }

    /// <summary>
    /// В наличии.
    /// </summary>
    [Description("В наличии")]
    public bool InStock { get; set; }

    /// <summary>
    /// Скидка.
    /// </summary>
    [Description("Скидка")]
    public decimal Discount { get; set; }

    /// <summary>
    /// Картинка.
    /// </summary>
    [Description("Картинка")]
    public string? Img { get; set; }

    /// <summary>
    /// Рейтинг.
    /// </summary>
    [Description("Рейтинг")]
    public int Rate { get; set; }

    /// <summary>
    /// Идентигфикатор каталога.
    /// </summary>
    [Description("Идентигфикатор каталога")]
    public long CatalogId { get; set; }

    /// <summary>
    /// Тэги.
    /// </summary>
    [Description("Тэги")]
    public List<TagResponse>? Tags { get; set; }
}