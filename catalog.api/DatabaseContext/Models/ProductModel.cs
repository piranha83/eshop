namespace Catalog.Api.DatabaseContext.Models;

using Infrastructure.Core.Abstractions;

///<inheritdoc/>
public class ProductModel : IEntity<long>, IUpdated
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Имя.
    /// </summary>
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// Описание.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Цена.
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// В наличии.
    /// </summary>
    public bool InStock { get; set; }
    
    /// <summary>
    /// Скидка.
    /// </summary>
    public decimal Discount { get; set; }
    
    /// <summary>
    /// Картинка.
    /// </summary>
    public string? Img { get; set; }

    /// <summary>
    /// Рейтинг.
    /// </summary>
    public int Rate { get; set; }

    /// <summary>
    /// Кто редактировал.
    /// </summary>
    public Guid? Updated { get; set; }
    
    /// <summary>
    /// Дата редактирования.
    /// </summary>
    public DateTimeOffset? UpdatedDate { get; set; }

    /// <summary>
    /// Идентигфикатор каталога.
    /// </summary>
    public long CatalogId { get; set; }

    /// <summary>
    /// Каталог.
    /// </summary>
    public CatalogModel Catalog { get; set; } = new();
    
    /// <summary>
    /// Тэги.
    /// </summary>
    public List<TagModel> Tags { get; set; } = new();
}