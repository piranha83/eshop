using Contract.Api.Payment;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.Api.DatabaseContext;
using Payment.Api.DatabaseContext.Models;
using Payment.Api.Features.PaymentStatus;

namespace Payment.Api.Features.PaymentRefund;

///<inheritdoc/>
internal class PaymentRefundEventConsumer(
    ApplicationDbContext dbContext,
    IBankSbpService bankService,
    ILogger<PaymentRefundEventConsumer> logger)
    : IConsumer<PaymentRefundEvent>
{
    ///<inheritdoc/>
    public async Task Consume(ConsumeContext<PaymentRefundEvent> context)
    {
        logger.LogInformation($"- Начать возврат {context.Message.OrderId} оплаты по сбп...");

        if (await dbContext.Payments
            .Filter(context.Message.OrderId, PaymentStatusType.Accepted)
            .FirstOrDefaultAsync(context.CancellationToken) is PaymentModel refundPayment
            && refundPayment.Status == PaymentStatusType.Accepted)
        {
            if (await bankService.RefundPayment(new RefundRequest
            {
                QrCodeId = refundPayment.QrCodeId!,
                Amount = refundPayment.Amount,
                Currency = refundPayment.Currency,
            }, context.CancellationToken) is RefundResponse refund)
            {
                refundPayment.Status = PaymentStatusType.RefundInProgress;

                await dbContext.SaveChangesAsync();
            }
        }

        logger.LogInformation($"✅ Возврат {context.Message.OrderId} оплаты по сбп.");
    }
}
