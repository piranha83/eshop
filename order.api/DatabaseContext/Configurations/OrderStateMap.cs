using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Api.Features.Order;

namespace Order.Api.DatabaseContext.Configurations;

///<inheritdoc/>
public class OrderStateMap : SagaClassMap<OrderState>
{
    protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
    {
        entity.HasKey(x => x.CorrelationId);
        entity.Property(x => x.State).HasMaxLength(32);
    }
}
