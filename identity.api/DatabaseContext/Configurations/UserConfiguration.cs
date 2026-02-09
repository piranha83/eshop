using Identity.Api.DatabaseContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Api.DatabaseContext.Configuration;

///<inheritdoc/>
public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    ///<inheritdoc/>
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ExternalId).HasMaxLength(128).IsRequired();
        builder.Property(x => x.UserName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.PasswordHash).IsRequired();
        builder.HasQueryFilter(b => !b.IsDeleted);
    }
}