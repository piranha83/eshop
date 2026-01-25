using System.Reflection;
using System.Text.Json;
using Catalog.Api.DatabaseContext;
using Catalog.Api.DatabaseContext.Models;
using Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Catalog.Api.Extensions;

internal static class Extensions
{
    internal static void UseImgFiles(this WebApplication application, string contentRootPath)
    {
        ArgumentNullException.ThrowIfNull(contentRootPath, nameof(contentRootPath));

        application.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(contentRootPath, "img")),
            RequestPath = "/img",
            DefaultContentType = "application/jpg",
        });
    }

    internal static async Task Init(this IApplicationBuilder application)
    {
        ArgumentNullException.ThrowIfNull(application, nameof(application));

        using var scope = application.ApplicationServices.CreateScope();
        await scope.EnsureCreated<ApplicationDbContext>();

        var catalog = ImportDemo();
        if (catalog == null) return;

        using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if(await dbContext.Catalogs.AnyAsync()) return;

        dbContext.Catalogs.Add(catalog);
        await dbContext.SaveChangesAsync();
    }

    internal static CatalogModel? ImportDemo()
    {
        var catalogId = 1;
        var tag = new TagModel { Id = 1, Name = "еда" };
        using var res = Assembly.GetExecutingAssembly().GetManifestResourceStream("Catalog.Api.products.json");
        if (res == null) return null;

        var catalog = JsonSerializer.Deserialize<Catalog>(res, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        catalog!.Products.ForEach(x =>
        {
            x.CatalogId = catalogId;
            x.Tags = [tag];
        });
        return new CatalogModel
        {
            Id = catalogId,
            Name = "Demo Store",
            Description = "Food cort demo store",
            Products = catalog.Products,
        };
    }

    private class Catalog
    {
        public List<ProductModel> Products { set; get; } = new();
    }
}