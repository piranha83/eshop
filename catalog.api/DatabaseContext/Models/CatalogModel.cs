namespace Catalog.Api.DatabaseContext.Models;

using System.ComponentModel;
using Infrastructure.Core.Abstractions;

///<inheritdoc/>
public class CatalogModel : IEntity<long>, IUpdated
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
    /// Картинка.
    /// </summary>
    [Description("Картинка")]
    public string? Img { get; set; }

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
    /// Продукты.
    /// </summary>
    [Description("Продукты")]
    public List<ProductModel> Products { get; set; } = new();
}