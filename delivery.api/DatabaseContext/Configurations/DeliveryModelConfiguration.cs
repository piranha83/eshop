using Delivery.Api.DatabaseContext.Models;
using Delivery.Api.Features.DeliveryStatus;
using Infrastructure.Core.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delivery.Api.DatabaseContext.Configuration;

///<inheritdoc/>
public class DeliveryModelConfiguration : IEntityTypeConfiguration<DeliveryModel>
{
    ///<inheritdoc/>
    public void Configure(EntityTypeBuilder<DeliveryModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.OrderId).IsUnique();
        builder.Property(x => x.DeliveryOfferId).IsRequired();
        builder.Property(x => x.AddressId).IsRequired();
        builder.Property(x => x.ClientId).IsRequired();
        builder.Property(x => x.Status).HasConversion<EnumToStringConverter<DeliveryStatusType>>();
    }
}