using Payment.Api.DatabaseContext.Models;
using FluentValidation;
using MassTransit;
using Payment.Api.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Contract.Api.Payment;
using Infrastructure.Core;

namespace Payment.Api.Features.PaymentProcess;

///<inheritdoc/>
internal class PaymentProcessEventConsumer(
    ApplicationDbContext dbContext,
    IBankSbpService bankService,
    IValidator<PaymentProcessEvent> validator,
    ILogger<PaymentProcessEventConsumer> logger)
    : IConsumer<PaymentProcessEvent>
{

    ///<inheritdoc/>
    public async Task Consume(ConsumeContext<PaymentProcessEvent> context)
    {
        logger.LogInformation($"- Начать оплату {context.Message.OrderId} по СБП...");

        var validationResult = await validator.ValidateAsync(context.Message, context.CancellationToken);
        if (!validationResult.IsValid)
        {
            throw new OperationCanceledException($"Логическая ошибка оплаты {context.Message.OrderId} по СБП.");
        }

        if (await dbContext.Payments.AnyAsync(x => x.OrderId == context.Message.OrderId, context.CancellationToken))
        {
            return;
        }

        var response = await bankService.GenerateQrCode(new QrCodeRequest
        {
            Description = $"Оплата заказа №{context.Message.OrderId} от {DateTimeOffset.UtcNow} на сумму {context.Message.Amount} {context.Message.CurrencyType}.",
            Amount = context.Message.Amount,
            Currency = context.Message.CurrencyType,
            TtlMinutes = Consts.QrCodesTTLInMinutes,
        }, context.CancellationToken);

        var payment = new PaymentModel
        {
            OrderId = context.Message.OrderId,
            ClientId = context.Message.ClientId,
            Amount = context.Message.Amount,
            Currency = context.Message.CurrencyType,
            Type = context.Message.PaymentType,
            QrCodeId = response.Id,
            QrCodePayload = response.Payload,
            Status = response.Status,
        };

        await dbContext.Payments.AddAsync(payment);

        // Проследить статус оплаты...хореография..
        /*
        await context.SchedulePublish<PaymentProcessWaitEvent>(
            DateTime.UtcNow + TimeSpan.FromMinutes(TtlMinutes), new
            {
                context.Message.OrderId,
                QrCodeId = response.Id,
            });
        */

        await dbContext.SaveChangesAsync();

        logger.LogInformation($"✅ Оплата {context.Message.OrderId} по СБП начата успешно.");
    }
}
