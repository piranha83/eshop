using Payment.Api.DatabaseContext.Models;
using Infrastructure.Core.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Contract.Api.Payment;
using Payment.Api.Features.PaymentStatus;

namespace Payment.Api.DatabaseContext.Configuration;

///<inheritdoc/>
public class PaymentModelConfiguration : IEntityTypeConfiguration<PaymentModel>
{
    ///<inheritdoc/>
    public void Configure(EntityTypeBuilder<PaymentModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.ClientId).IsUnique();
        builder.Property(x => x.OrderId).IsRequired();
        builder.Property(x => x.Amount).IsRequired();
        builder.Property(x => x.Currency).HasConversion<EnumToStringConverter<CurrencyType>>();
        builder.Property(x => x.Type).HasConversion<EnumToStringConverter<PaymentType>>();
        builder.Property(x => x.Status).HasConversion<EnumToStringConverter<PaymentStatusType>>();
        builder.Property(x => x.QrCodeId).HasMaxLength(64);
        builder.Property(x => x.QrCodePayload).HasMaxLength(128);
    }
}