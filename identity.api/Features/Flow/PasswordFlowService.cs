using System.Security.Claims;
using Identity.Api.DatabaseContext;
using Identity.Api.DatabaseContext.Models;
using Identity.Api.Featres.User;
using Infrastructure.Core.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Api.Featres.Flow;

///<inheritdoc/>
internal class PasswordFlowService(
    IUserService userService,
    IHttpContextAccessor httpContextAccessor,
    IOpenIddictTokenManager tokenManager,
    IOpenIddictScopeManager scopeManager)
    : IFlowService
{
    static readonly string Scheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;

    ///<inheritdoc/>
    public async Task Token(CancellationToken ct = default)
    {
        var httpContext = httpContextAccessor.HttpContext;
        ArgumentNullException.ThrowIfNull(httpContext);

        var request = httpContext.GetOpenIddictServerRequest();
        ArgumentNullException.ThrowIfNull(request);

        if (!request.IsPasswordGrantType()
            || string.IsNullOrWhiteSpace(request.Username)
            || string.IsNullOrWhiteSpace(request.Password))
        {
            await httpContext.ForbidAsync(Scheme);
            return;
        }

        var user = await userService.SignIn(request.Username, request.Password, ct);
        if (user == null)
        {
            await httpContext.ForbidAsync(Scheme);
            return;
        }

        // Один token на сессию пользователя.
        await tokenManager.RevokeBySubjectAsync(user.ExternalId, ct);

        // Создание утверждения, которое будет использоваться OpenIddict для токена авторизации.
        var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);

        identity.SetClaim(Claims.Subject, user.ExternalId)
            .SetClaim(Claims.Name, user.UserName)
            .SetClaims(Claims.Role, [user.Roles.ToString()])
            .SetResources(await scopeManager.ListResourcesAsync(request.GetScopes(), ct).ToListAsync())
            .SetDestinations(_ => [Destinations.AccessToken]);

        await httpContext.SignInAsync(Scheme, principal: new ClaimsPrincipal(identity));
    }
}