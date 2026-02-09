using System.Security.Claims;
using Identity.Api.DatabaseContext.Models;
using Identity.Api.Featres.User;
using Infrastructure.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Api.Featres.Flow;

///<inheritdoc/>
internal class PasswordFlowService(
    IHttpContextAccessor httpContextAccessor,
    IUserService userService,
    IOpenIddictTokenManager tokenManager,
    IOpenIddictApplicationManager applicationManager,
    IConfiguration configuration)
    : IFlowService
{
    static readonly string Scheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;

    ///<inheritdoc/>
    public async Task Token(CancellationToken cancellationToken = default)
    {
        var context = httpContextAccessor.HttpContext;
        ArgumentNullException.ThrowIfNull(context);

        var request = context.GetOpenIddictServerRequest();
        ArgumentNullException.ThrowIfNull(request);

        var origin = configuration.GetSection(Consts.ApplicationOrigin).Value;
        ArgumentException.ThrowIfNullOrEmpty(origin);

        // Идентификация.
        UserEntity? user = null;
        if (request.IsPasswordGrantType())
        {
            user = await userService.FindByName(request.Username!);
        }

        if (user == null)
        {
            await context.ForbidAsync(Scheme);
            return;
        }

        // Аутентификация.
        if (await userService.CheckPasswordSignIn(user, request.Password!) == false)
        {
            await context.ForbidAsync(Scheme);
            return;
        }

        // Один token на сессию пользователя.
        await tokenManager.RevokeBySubjectAsync(user.ExternalId, cancellationToken);

        // Создание утверждения, которое будет использоваться OpenIddict для токена авторизации.
        var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);

        identity.SetClaim(Claims.Subject, user.ExternalId)
            .SetClaim(Claims.Name, user.UserName)
            .SetClaim(Claims.Nonce, origin)
            .SetResources(request.GetResources())
            .SetScopes(new[] { /*Scopes.OpenId, Scopes.OfflineAccess,*/ Scopes.Roles })
            .SetClaims(Claims.Role, [.. await userService.GetRoles(user, cancellationToken)]);

        await context.SignInAsync(Scheme, principal: new ClaimsPrincipal(identity));
    }

    ///<inheritdoc/>
    public ValueTask<long> End(CancellationToken cancellationToken = default) => throw new NotImplementedException();

    ///<inheritdoc/>
    public async Task AddApplication(CancellationToken cancellationToken = default)
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
                    Permissions.GrantTypes.Password,
                    //Permissions.GrantTypes.RefreshToken,
                    Scopes.Roles,
                    //Scopes.OpenId
                },
            }, cancellationToken);
        }
    }
}