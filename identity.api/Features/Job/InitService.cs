using Infrastructure.Core.Extensions;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Api.Featres.Job;

///<inheritdoc/>
internal class InitService(
    IOpenIddictApplicationManager applicationManager,
    IOpenIddictScopeManager scopeManager,
    IConfiguration configuration)
    : IInitService
{
    ///<inheritdoc/>
    public async Task Init(CancellationToken cancellationToken = default)
    {
        if (await applicationManager.FindByClientIdAsync("ClientPsw", cancellationToken) is null)
        {
            await applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "ClientPsw",
                ClientType = ClientTypes.Public,
                DisplayName = "Password Flow Application Client",
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.Endpoints.EndSession,
                    Permissions.GrantTypes.Password,
                    Permissions.Scopes.Roles,
                    //Permissions.GrantTypes.RefreshToken,
                    Permissions.Prefixes.Scope + "catalog-api",
                },
            }, cancellationToken);
        }

        if (await scopeManager.FindByNameAsync("catalog-api", cancellationToken) is null)
        {
            await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = "catalog-api",
                DisplayName = "Catalog Api Audience",
                Resources = { configuration.GetCatalogApiUrl() }
            });
        }
    }
}