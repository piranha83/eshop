using Payment.Api.Features.PaymentProcess;
using Infrastructure.Core.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.Api.DatabaseContext;
using Payment.Api.Featres.Job;
using Payment.Api.Features.TochkaBank;
using Payment.Api.Features;
using Microsoft.Extensions.Options;
using Payment.Api.Features.PaymentStatus;
using Payment.Api.DatabaseContext.Models;

namespace Payment.Api.Extensions;

/// <summary>
/// Extensions.
/// </summary>
public static class Extensions
{
    public static async Task<PaymentModel?> FindByOrderId(this IQueryable<PaymentModel> payments, Guid orderId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payments);

        return await payments.SingleOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);
    }
}