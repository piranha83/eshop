using Catalog.Api.DatabaseContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Api.DatabaseContext.Configuration;

///<inheritdoc/>
public class CatalogModelConfiguration : IEntityTypeConfiguration<CatalogModel>
{
    ///<inheritdoc/>
    public void Configure(EntityTypeBuilder<CatalogModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Img).HasMaxLength(500);

        builder.HasMany(x => x.Products)
           .WithOne(x => x.Catalog)
           .HasForeignKey(x => x.CatalogId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}