namespace Catalog.Api.DatabaseContext.Models;

using Infrastructure.Core.Abstractions;

///<inheritdoc/>
public class CatalogModel : IEntity<long>, IUpdated
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
    /// Картинка.
    /// </summary>
    public string? Img { get; set; }

    /// <summary>
    /// Кто редактировал.
    /// </summary>
    public Guid? Updated { get; set; }
    
    /// <summary>
    /// Дата редактирования.
    /// </summary>
    public DateTimeOffset? UpdatedDate { get; set; }

    /// <summary>
    /// Продукты.
    /// </summary>
    public List<ProductModel> Products { get; set; } = new();
}