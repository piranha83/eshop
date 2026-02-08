namespace Catalog.Api.DatabaseContext.Models;

using System.ComponentModel;
using Infrastructure.Core.Abstractions;

///<inheritdoc/>
public class TagModel : IEntity<long>, IUpdated
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
    public List<ProductModel> Products { get; set; } = default!;
}