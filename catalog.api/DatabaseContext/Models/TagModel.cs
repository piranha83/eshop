namespace Catalog.Api.DatabaseContext.Models;

using Infrastructure.Core.Abstractions;

///<inheritdoc/>
public class TagModel : IEntity<long>, IUpdated
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