using Contract.Api.Payment;
using Infrastructure.Core.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.Api.DatabaseContext;
using Payment.Api.DatabaseContext.Models;

namespace Payment.Api.Features.PaymentStatus;

///<inheritdoc/>
internal class PaymentStatusEventConsumer(
    ApplicationDbContext dbContext,
    IBankSbpService bankService,
    ILogger<PaymentStatusEventConsumer> logger)
    : IConsumer<PaymentStatusEvent>
{
    ///<inheritdoc/>
    public async Task Consume(ConsumeContext<PaymentStatusEvent> context)
    {
        logger.LogInformation($"- Оплата по сбп {context.Message.OrderId} проверка статуса...");

        var qrCodeId = await dbContext.Payments.Where(x => x.OrderId == context.Message.OrderId)
            .Select(x => x.QrCodeId)
            .FirstOrDefaultAsync(context.CancellationToken);

        if (qrCodeId != null)
        {
            switch (await bankService.GetPaymentStatus(qrCodeId, context.CancellationToken))
            {
                // оплата по сбп завершена успешно
                case PaymentStatusType.Accepted:
                    if (await dbContext.Payments.ForUpdate().FirstOrDefaultAsync(x =>
                        x.OrderId == context.Message.OrderId &&
                        x.Status == PaymentStatusType.InProgress &&
                        x.Type == PaymentType.SBP,
                        context.CancellationToken) is PaymentModel acceptedPayment)
                    {
                        acceptedPayment.Status = PaymentStatusType.Accepted;
                        dbContext.Update(acceptedPayment);

                        await context.Publish<PaymentReceivedEvent>(new
                        {
                            acceptedPayment.OrderId,
                            PaymentId = acceptedPayment.Id,
                        }, context.CancellationToken);

                        await dbContext.SaveChangesAsync(context.CancellationToken);

                        logger.LogInformation($"✅ Оплата по сбп {context.Message.OrderId} завершена.");
                    }
                    break;

                // возврат
                case PaymentStatusType.RefundInProgress:
                break;

                // оплата по сбп отклонена, не оплачена
                default:
                    if (await dbContext.Payments.ForUpdate().FirstOrDefaultAsync(x =>
                        x.OrderId == context.Message.OrderId &&
                        x.Status == PaymentStatusType.InProgress &&
                        x.Type == PaymentType.SBP,
                        context.CancellationToken) is PaymentModel rejectedPayment)
                    {
                        rejectedPayment.Status = PaymentStatusType.Rejected;
                        dbContext.Update(rejectedPayment);

                        await context.Publish<PaymentCancelledEvent>(new
                        {
                            rejectedPayment.OrderId,
                            PaymentId = rejectedPayment.Id,
                        }, context.CancellationToken);

                        await dbContext.SaveChangesAsync(context.CancellationToken);

                        logger.LogInformation($"✅ Оплата по сбп {context.Message.OrderId} отклонена.");
                    }
                    break;
            }
        }

        logger.LogInformation($"✅ Оплата по сбп {context.Message.OrderId} проверка статуса.");
    }
}
