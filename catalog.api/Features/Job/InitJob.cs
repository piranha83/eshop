using System.Text.Json;
using Catalog.Api.DatabaseContext;
using Catalog.Api.DatabaseContext.Models;
using Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Featres.Job;

///<inheritdoc/>
internal partial class InitJob(
    IServiceProvider serviceProvider,
    ILogger<InitJob> logger)
    : BackgroundService
{
    ///<inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Начало задачи...");

            using var scope = serviceProvider.CreateScope();
            await scope.EnsureCreated<ApplicationDbContext>();

            var catalog = ImportDemo();
            if (catalog == null) return;

            using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (await dbContext.Catalogs.AnyAsync()) return;

            dbContext.Catalogs.Add(catalog);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Задача завершена.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка задачи.");
        }
    }

    internal static CatalogModel? ImportDemo()
    {
        var tag = new TagModel { Name = "еда" };
        using var res = typeof(InitJob).Assembly.GetManifestResourceStream("Catalog.Api.products.json");
        if (res == null) return null;

        var catalog = JsonSerializer.Deserialize<Catalog>(res, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        catalog!.Products.ForEach(x =>
        {
            x.Tags = [tag];
        });
        return new CatalogModel
        {
            Name = "Demo Store",
            Description = "Food cort demo store",
            Products = catalog.Products,
        };
    }
}