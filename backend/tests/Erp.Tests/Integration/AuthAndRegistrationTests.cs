using System.Net;
using System.Net.Http.Json;
using System.Text;
using Erp.Modules.Core.Endpoints;
using Erp.SharedKernel.Responses;
using Erp.Tests.Fixtures;
using Erp.Tests.Helpers;
using Xunit;

namespace Erp.Tests.Integration;

[Collection("IntegrationTests")]
public class AuthAndRegistrationTests
{
    private readonly ErpTestWebApplicationFactory _factory;

    public AuthAndRegistrationTests(ErpTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task RegisterPatient_WithStringEnums_Succeeds_And_ReturnsValidJwt()
    {
        var client = _factory.CreateClient();
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var email = $"patient_{suffix}@erp.local";
        var phone = $"+2010{Random.Shared.Next(10000000, 99999999)}";
        var nationalId = $"29{Random.Shared.NextInt64(100000000000, 999999999999)}";

        // JSON payload sending Gender and BloodType as explicit readable strings ("Male", "APositive")
        var jsonPayload = $$"""
        {
          "email": "{{email}}",
          "phoneNumber": "{{phone}}",
          "password": "Password123!",
          "fullName": "Test Patient {{suffix}}",
          "nationalId": "{{nationalId}}",
          "dateOfBirth": "1994-06-15",
          "gender": "Male",
          "bloodType": "APositive",
          "weightKg": 76.5,
          "heightCm": 178.0,
          "emergencyContactName": "Emergency Contact",
          "emergencyContactPhone": "+201011112222"
        }
        """;

        var response = await client.PostAsync(
            "/api/core/auth/register-patient",
            new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<AuthEndpoints.RegisterPatientResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
        Assert.NotEqual(Guid.Empty, result.UserId);
        Assert.NotEqual(Guid.Empty, result.PatientId);
        Assert.Equal("Patient", result.Role);

        // Verify patient can now log in
        var loginResponse = await client.PostAsJsonAsync("/api/core/auth/login", new AuthEndpoints.LoginRequest(
            email,
            "Password123!"), TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthEndpoints.LoginResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(loginResult);
        Assert.Equal(result.UserId, loginResult.UserId);
        Assert.Equal(result.PatientId, loginResult.PatientId);
    }

    [Fact]
    public async Task RegisterPharmacist_Succeeds_And_ReturnsValidToken()
    {
        var client = _factory.CreateClient();
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var email = $"pharmacist_{suffix}@erp.local";
        var phone = $"+2010{Random.Shared.Next(10000000, 99999999)}";

        var response = await client.PostAsJsonAsync("/api/core/auth/register-pharmacist", new AuthEndpoints.RegisterPharmacistRequest(
            Email: email,
            PhoneNumber: phone,
            Password: "Password123!",
            FullName: $"Dr. Pharmacist {suffix}",
            TenantId: null), TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<AuthEndpoints.RegisterPharmacistResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
        Assert.NotEqual(Guid.Empty, result.UserId);
        Assert.Equal("Pharmacist", result.Role);
    }

    [Fact]
    public async Task Login_WithSeededPatient_Succeeds_And_ReturnsClaims()
    {
        var client = _factory.CreateClient();

        var login = await TestAuthHelper.LoginAsync(client, "patient@erp.local", "Password123!");

        Assert.NotNull(login);
        Assert.False(string.IsNullOrWhiteSpace(login.Token));
        Assert.Equal("Patient", login.Role);
        Assert.NotNull(login.PatientId);
    }

    [Fact]
    public async Task Login_WithSeededPharmacist_Succeeds_And_VerifiesJwtClaims()
    {
        var client = _factory.CreateClient();

        var login = await TestAuthHelper.LoginAsync(client, "pharmacist@erp.local", "Password123!");

        Assert.NotNull(login);
        Assert.False(string.IsNullOrWhiteSpace(login.Token));
        Assert.Equal("Pharmacist", login.Role);
        Assert.NotNull(login.TenantId);
        Assert.Equal(Erp.Modules.Core.Data.CoreDbSeeder.DefaultTenantId, login.TenantId.Value);

        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(login.Token);

        Assert.Equal(login.UserId.ToString(), jwt.Claims.First(c => c.Type == "sub").Value);
        Assert.Equal(login.TenantId.Value.ToString(), jwt.Claims.First(c => c.Type == "tenant_id").Value);
        Assert.Equal("Pharmacist", jwt.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.Role || c.Type == "role").Value);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/core/auth/login", new AuthEndpoints.LoginRequest(
            "patient@erp.local",
            "WrongPassword!"), TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RegisterPatient_DuplicateEmail_ReturnsConflict()
    {
        var client = _factory.CreateClient();

        var jsonPayload = """
        {
          "email": "patient@erp.local",
          "phoneNumber": "+201099999999",
          "password": "Password123!",
          "fullName": "Duplicate Patient",
          "nationalId": "29999999999999",
          "dateOfBirth": "1990-01-01",
          "gender": "Female",
          "bloodType": "BPositive",
          "weightKg": 60.0,
          "heightCm": 165.0
        }
        """;

        var response = await client.PostAsync(
            "/api/core/auth/register-patient",
            new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var err = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(err);
        Assert.Contains("Email", err.Message);
    }
}
