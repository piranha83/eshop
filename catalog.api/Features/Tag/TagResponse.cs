using System.ComponentModel;

namespace Catalog.Api.Features.Product;

/// <summary>
/// Тэг.
/// </summary>
[Description("Тэг")]
public class TagResponse
{
    /// <summary>
    /// Имя.
    /// </summary>
    [Description("Имя")]
    public string Name { get; set; } = default!;
}