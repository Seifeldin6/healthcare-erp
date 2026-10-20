using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Erp.Modules.Core.Endpoints;
using Erp.Tests.Fixtures;

namespace Erp.Tests.Helpers;

public static class TestAuthHelper
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static async Task<AuthEndpoints.LoginResponse> LoginAsync(
        HttpClient client,
        string emailOrPhone,
        string password = "Password123!")
    {
        var response = await client.PostAsJsonAsync("/api/core/auth/login", new AuthEndpoints.LoginRequest(
            emailOrPhone,
            password), JsonOptions);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthEndpoints.LoginResponse>(JsonOptions);
        return result ?? throw new InvalidOperationException("Failed to deserialize LoginResponse.");
    }

    public static HttpClient CreateAuthenticatedClient(
        ErpTestWebApplicationFactory factory,
        string token)
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}
