using MassTransit;
using Infrastructure.Core.Extensions;
using Payment.Api.DatabaseContext.Models;
using Contract.Api.Payment;
using Payment.Api.DatabaseContext;
using Payment.Api.Extensions;
using Payment.Api.Features.PaymentStatus;

namespace Payment.Api.Features.PaymentStatusSet;

///<inheritdoc/>
[Obsolete("Используем для хуков банка, в sandox не работает")]
internal class PaymentStatusSetEventConsumer(
    ApplicationDbContext dbContext,
    ILogger<PaymentStatusSetEventConsumer> logger)
    : IConsumer<PaymentStatusSetEvent>
{
    ///<inheritdoc/>
    public async Task Consume(ConsumeContext<PaymentStatusSetEvent> context)
    {
        logger.LogInformation($"- Оплата по сбп {context.Message.OrderId} статус изменился на {context.Message.PaymentStatus}...");

        switch (context.Message.PaymentStatus)
        {
            // оплата по сбп завершена успешно
            case PaymentStatusType.Accepted:
                if (await dbContext.Payments.ForUpdate().FindByOrderId(context.Message.OrderId, context.CancellationToken) is PaymentModel acceptedPayment
                && acceptedPayment.Status == PaymentStatusType.InProgress)
                {
                    acceptedPayment.Status = PaymentStatusType.Accepted;

                    await context.Publish<PaymentReceivedEvent>(new
                    {
                        acceptedPayment.OrderId,
                        PaymentId = acceptedPayment.Id,
                    });
                    await dbContext.SaveChangesAsync();

                    logger.LogInformation($"✅ Оплата по сбп {context.Message.OrderId} завершена.");
                }
                break;

            // оплата по сбп отклонена, не оплачена
            case PaymentStatusType.Rejected:
            case PaymentStatusType.NotStarted:
                if (await dbContext.Payments.ForUpdate().FindByOrderId(context.Message.OrderId, context.CancellationToken) is PaymentModel rejectedPayment
                && rejectedPayment.Status != PaymentStatusType.Accepted)
                {
                    rejectedPayment.Status = PaymentStatusType.Rejected;

                    await context.Publish<PaymentCancelledEvent>(new
                    {
                        rejectedPayment.OrderId,
                        PaymentId = rejectedPayment.Id,
                    });
                    await dbContext.SaveChangesAsync();

                    logger.LogInformation($"✅ Оплата по сбп {context.Message.OrderId} отклонена.");
                }
                break;

            case PaymentStatusType.InProgress:
                logger.LogInformation($"✅ Ждем оплату по сбп {context.Message.OrderId}.");
                break;

            default:
                logger.LogWarning($"❌ Оплата по сбп {context.Message.OrderId} статус не обработан {context.Message.PaymentStatus}");
                break;
        }
    }
}
