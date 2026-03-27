using AutoMapper;
using Contract.Api.Order;
using FluentValidation;
using Infrastructure.Core.Extensions;
using Infrastructure.Core.Features.Context;
using MassTransit;

namespace Order.Api.Features.Order;

///<inheritdoc/>
public sealed class OrderServcie(
    IPublishEndpoint endpoint,
    IValidator<OrderSubmittedEvent> validator,
    IContextAccessor contextAccessor)
    : IOrderServcie
{
    ///<inheritdoc/>
    public async Task<Guid> Create(OrderModel reqest, CancellationToken cancellationToken)
    {
        var @event = reqest with
        {
            OrderId = Guid.NewGuid(),
            ClientId = Guid.NewGuid()/*contextAccessor.GetUser()!.UserId.GetValueOrDefault()*/
        } as OrderSubmittedEvent;
        await validator.Validate<OrderSubmittedEvent>(@event);
        await endpoint.Publish(@event, cancellationToken);
        return @event.OrderId;
    }
}