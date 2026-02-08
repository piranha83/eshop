using Catalog.Api.DatabaseContext.Models;

namespace Catalog.Api.Featres.Job;

///<inheritdoc/>
internal partial class Catalog
{
    public List<ProductModel> Products { set; get; } = new();
}