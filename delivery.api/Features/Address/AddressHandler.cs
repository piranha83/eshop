using Microsoft.Extensions.Options;

namespace Delivery.Api.Features.Address;

///<inheritdoc/>
internal class AddressHandler(IOptions<AddressOption> options) : DelegatingHandler
{
    ///<inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(options.Value.Token);
        request.Headers.Add("master-token", options.Value.Token);
        return await base.SendAsync(request, cancellationToken);
    }
}