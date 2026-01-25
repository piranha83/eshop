namespace Catalog.Api.DatabaseContext;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Catalog.Api.DatabaseContext.Models;
using SystemTextJson.FluentApi;

///<inheritdoc/>
public static class JsonConfiguration
{
    ///<inheritdoc/>
    public static JsonSerializerOptions Default()
    {
        var options = new JsonSerializerOptions();

        options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        options.WriteIndented = true;
        options.PropertyNameCaseInsensitive = true;
        options.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        options.ConfigureDefaultTypeResolver(x => x
            .Entity<ProductModel>()
            .Property(p => p.CatalogId).IsIgnored()
            .Property(p => p.Catalog).IsIgnored()
            .Property(p => p.Updated).IsIgnored());

        options.ConfigureDefaultTypeResolver(x => x
            .Entity<CatalogModel>()
            .Property(p => p.Updated).IsIgnored());

        options.ConfigureDefaultTypeResolver(x => x
            .Entity<TagModel>()
            .Property(p => p.Updated).IsIgnored());

        return options;
    }
}