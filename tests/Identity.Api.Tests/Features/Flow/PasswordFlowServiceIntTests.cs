using System.Net;
using System.Text.Json.Serialization;
using Infrastructure.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Api.Tests.Featres.Flow;

[Trait("Category", "Integration")]
public class PasswordFlowServiceIntTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private class Response
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }

    private static readonly string url = $"https://localhost:7255/{Consts.TokenEndpointUrl}";

    [Fact]
    public async Task Token_ReturnsTokenAndOkStatusCode()
    {
        // Arrange
        var values = new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", "ClientPsw" },
            { "username", "Client" },
            { "password", "7k=9r8F" },
            { "scope", "catalog-api" },
        };
        using var client = factory.CreateClient();

        // Act
        using var response = await client.PostAsync(url, new FormUrlEncodedContent(values));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Content);
        var resp = await response.Content.ReadFromJsonAsync<Response>();
        Assert.NotNull(resp);
        Assert.NotNull(resp.AccessToken);
    }

    
    [Fact]
    public async Task InvalidUserName_ReturnsInvalidGrant()
    {
        // Arrange
        var values = new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", "ClientPsw" },
            { "username", "invalid_Client" },
            { "password", "7k=9r8F" },
            { "scope", "catalog-api" },
        };
        using var client = factory.CreateClient();

        // Act
        using var response = await client.PostAsync(url, new FormUrlEncodedContent(values));

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        var resp = await response.Content.ReadFromJsonAsync<Response>();
        Assert.NotNull(resp);
        Assert.Equal(Errors.InvalidGrant, resp.Error);
    }

    [Fact]
    public async Task InvalidClientName_ReturnsInvalidClient()
    {
        // Arrange
        var values = new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", "invalid_ClientPsw" },
            { "username", "Client" },
            { "password", "7k=9r8F" },
            { "scope", "catalog-api" },
        };
        using var client = factory.CreateClient();

        // Act
        using var response = await client.PostAsync(url, new FormUrlEncodedContent(values));

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        var resp = await response.Content.ReadFromJsonAsync<Response>();
        Assert.NotNull(resp);
        Assert.Equal(Errors.InvalidClient, resp.Error);
    }
}