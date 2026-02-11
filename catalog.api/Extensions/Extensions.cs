using Infrastructure.Core;
using Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Catalog.Api.Extensions;

internal static class Extensions
{
    internal static void UseImgFiles(this WebApplication application, string contentRootPath)
    {
        ArgumentNullException.ThrowIfNull(contentRootPath, nameof(contentRootPath));

        application.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(contentRootPath, "img")),
            RequestPath = "/img",
            DefaultContentType = "application/jpg",
        });
    }

    internal static void AddClientFlow(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        // Register the OpenIddict validation components.
        serviceCollection.AddOpenIddict().AddValidation(options =>
        {
            options.SetIssuer(configuration.GetIdentityApiUrl());
            options.AddAudiences(configuration.GetCatalogApiUrl());
            //options.AddEncryptionCertificate(typeof(Program).Assembly, "Catalog.Api.server-encryption-certificate.pfx", null);
            // Register the System.Net.Http integration.
            options.UseSystemNetHttp().ConfigureHttpClientHandler(handler => handler
                .ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator);//todo
            // Register the ASP.NET Core host.
            options.UseAspNetCore();
        });
        serviceCollection.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        serviceCollection.AddAuthorization(options =>
        {
            options.AddPolicy(Consts.Policy.Admin, x => x
                .RequireRole(Claims.Role, nameof(Consts.ClaimsRoles.Admin))
                .AuthenticationSchemes.Add(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme));
            options.AddPolicy(Consts.Policy.Viewer, x => x
                .RequireRole(Claims.Role, nameof(Consts.ClaimsRoles.Viewer))
                .AuthenticationSchemes.Add(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme));
        });
    }

    internal static void AddSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen();
    }

    internal static IApplicationBuilder UseClientFlow(this IApplicationBuilder app)
    {
        return app.UseAuthentication().UseAuthorization();
    }
}