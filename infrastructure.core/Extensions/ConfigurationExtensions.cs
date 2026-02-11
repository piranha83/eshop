using Microsoft.Extensions.Configuration;

namespace Infrastructure.Core.Extensions;

/// <summary>
/// ConfigurationExtensions.
/// </summary>
public static class ConfigurationExtensions
{
    public static string GetCatalogApiUrl(this IConfiguration configuration) =>
        configuration.GetString("CatalogApi:Url");

    public static string GetIdentityApiUrl(this IConfiguration configuration) =>
        configuration.GetString("IdentityApi:Url");

    private static string GetString(this IConfiguration configuration, string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);

        var value = configuration.GetSection(key)?.Value;
        ArgumentException.ThrowIfNullOrEmpty(value);

        return value;
    }
}