using Catalog.Api.DatabaseContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Api.DatabaseContext.Configuration;

///<inheritdoc/>
public class ProductModelConfiguration : IEntityTypeConfiguration<ProductModel>
{
    public void Configure(EntityTypeBuilder<ProductModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.Img).HasMaxLength(500);

        builder.HasMany(x => x.Tags)
           .WithMany(x => x.Products)
           .UsingEntity(x => x.ToTable("ProductTags"));
    }
}