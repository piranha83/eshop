namespace Catalog.Api.DatabaseContext.Models;

using System.ComponentModel;
using Infrastructure.Core.Abstractions;

///<inheritdoc/>
public class ProductModel : IEntity<long>, IUpdated
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
    /// Кто редактировал.
    /// </summary>
    [Description("Кто редактировал")]
    public Guid? Updated { get; set; }
    
    /// <summary>
    /// Дата редактирования.
    /// </summary>
    [Description("Дата редактирования")]
    public DateTimeOffset? UpdatedDate { get; set; }

    /// <summary>
    /// Идентигфикатор каталога.
    /// </summary>
    [Description("Идентигфикатор каталога")]
    public long CatalogId { get; set; }

    /// <summary>
    /// Каталог.
    /// </summary>
    [Description("Каталог")]
    public CatalogModel Catalog { get; set; } = new();
    
    /// <summary>
    /// Тэги.
    /// </summary>
    [Description("Тэги")]
    public List<TagModel> Tags { get; set; } = new();
}