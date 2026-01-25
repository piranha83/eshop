using Catalog.Api.DatabaseContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Api.DatabaseContext.Configuration;

///<inheritdoc/>
public class TagModelConfiguration : IEntityTypeConfiguration<TagModel>
{
    ///<inheritdoc/>
    public void Configure(EntityTypeBuilder<TagModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
    }
}