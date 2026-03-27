using Payment.Api.DatabaseContext.Models;
using Contract.Api.Payment;
using Payment.Api.Features.PaymentStatus;

namespace Payment.Api.Features;

/// <summary>
/// Extensions.
/// </summary>
public static class Extensions
{
    public static IQueryable<PaymentModel> Filter(this IQueryable<PaymentModel> payments, Guid orderId, PaymentStatusType status)
    {
        ArgumentNullException.ThrowIfNull(payments);

        return payments.Where(x => x.OrderId == orderId && x.Type == PaymentType.SBP && x.Status == status);
    }
}