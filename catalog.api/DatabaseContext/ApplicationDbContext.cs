namespace Catalog.Api.DatabaseContext;

using System.Reflection;
using Catalog.Api.DatabaseContext.Models;
using Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;

///<inheritdoc/>
internal class ApplicationDbContext : DbContext
{
    public DbSet<CatalogModel> Catalogs { get; set; }

    public DbSet<ProductModel> Products { get; set; }

    public DbSet<TagModel> Tags { get; set; }


    ///<inheritdoc/>
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    ///<inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("catalog");
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}