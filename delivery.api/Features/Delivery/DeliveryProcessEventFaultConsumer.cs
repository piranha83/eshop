using Contract.Api.Delivery;
using MassTransit;
using Order.Api.DatabaseContext;

namespace Delivery.Api.Features.Delivery;

///<inheritdoc/>
internal class DeliveryProcessEventFaultConsumer(
    ApplicationDbContext dbContext,
    ILogger<DeliveryProcessEventFaultConsumer> logger)
    : IConsumer<Fault<DeliveryProcessEvent>>
{
    ///<inheritdoc/>
    public async Task Consume(ConsumeContext<Fault<DeliveryProcessEvent>> context)
    {
        logger.LogInformation("Отмена заказа: начало...");

        await context.Publish<DeliveryCancelledEvent>(new
        {
            context.Message.Message.OrderId,
            context.Message.Message,
        }, context.CancellationToken);
        await dbContext.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation("❌ Отмена заказа: закончен успешно.");
    }
}
