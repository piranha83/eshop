using Contract.Api.Payment;
using MassTransit;
using Payment.Api.DatabaseContext;

namespace Payment.Api.Features.PaymentProcess;

///<inheritdoc/>
internal class PaymentProcessEventFaultConsumer(
    ApplicationDbContext dbContext,
    ILogger<PaymentProcessEventFaultConsumer> logger)
    : IConsumer<Fault<PaymentProcessEvent>>
{
    ///<inheritdoc/>
    public async Task Consume(ConsumeContext<Fault<PaymentProcessEvent>> context)
    {
        logger.LogInformation($"- Отмена оплаты {context.Message.Message.OrderId}...");

        if (context.Message.Exceptions != null)
        {
            // Причина отмены:
            foreach (var ex in context.Message.Exceptions)
            {
                logger.LogError(ex.Message);
            }
        }

        await context.Publish<PaymentCancelledEvent>(context.Message.Message, context.CancellationToken);
        await dbContext.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation($"❌ Отмена оплаты {context.Message.Message.OrderId}.");
    }
}
