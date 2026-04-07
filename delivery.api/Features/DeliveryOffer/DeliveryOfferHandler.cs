using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace Delivery.Api.Features.DeliveryOffer;

///<inheritdoc/>
internal class DeliveryOfferHandler(IOptions<DeliveryOfferOption> options) : DelegatingHandler
{
    ///<inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(options.Value);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(options.Value.Token);

        // Sandbox mode:
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Value.Token);

        return await base.SendAsync(request, cancellationToken);
    }
}