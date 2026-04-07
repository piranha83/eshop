using Contract.Api.Delivery;
using Delivery.Api.DatabaseContext.Models;
using Delivery.Api.Features.DeliveryOffer;
using Delivery.Api.Features.DeliveryStatus;
using FluentValidation;
using MassTransit;
using Order.Api.DatabaseContext;

namespace Delivery.Api.Features.Delivery;

///<inheritdoc/>
internal class DeliveryProcessEventConsumer(
    ApplicationDbContext dbContext,
    IDeliveryOfferService deliveryOfferService,
    IValidator<DeliveryProcessEvent> validator,
    ILogger<DeliveryProcessEventConsumer> logger)
    : IConsumer<DeliveryProcessEvent>
{
    ///<inheritdoc/>
    public async Task Consume(ConsumeContext<DeliveryProcessEvent> context)
    {
        logger.LogInformation($"- Принять заказ {context.Message.OrderId}...");

        var validationResult = await validator.ValidateAsync(context.Message, context.CancellationToken);
        if (!validationResult.IsValid)
        {
            throw new OperationCanceledException($"Принять заказ {context.Message.OrderId} нельзя, логическая ошибка.");
        }

        var deliveryExternal = await deliveryOfferService.Create(new DeliveryOfferCreateRequest
        {
            Address = context.Message.Address,
            OrderId = context.Message.OrderId,
            CartItems = context.Message.CartItems,
            FirstName = context.Message.FirstName,
            Phone = context.Message.Phone,
        }, context.CancellationToken);

        var acceptDelivery = new DeliveryModel
        {
            OrderId = context.Message.OrderId,
            ClientId = context.Message.ClientId,
            DeliveryOfferId = deliveryExternal.DeliveryOfferId,
            AddressId = deliveryExternal.AddressId,
            Status = DeliveryStatusType.Accepted,
        };

        await dbContext.Deliveries.AddAsync(acceptDelivery, context.CancellationToken);
        await context.Publish<DeliveryAcceptedEvent>(new
        {
            context.Message.OrderId,
            DeliveryId = acceptDelivery.Id,
        }, context.CancellationToken);
        await dbContext.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation($"✅ Принял заказ {context.Message.OrderId} успешно.");
    }
}
