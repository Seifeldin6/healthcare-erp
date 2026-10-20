using System.Net;
using System.Net.Http.Json;
using Erp.Modules.Core.Endpoints;
using Erp.Modules.TelePharmacy.Data;
using Erp.Modules.TelePharmacy.Endpoints;
using Erp.Tests.Fixtures;
using Erp.Tests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Erp.Tests.Integration;

[Collection("IntegrationTests")]
public class ConfigurableVideoHostingTests
{
    private readonly ErpTestWebApplicationFactory _factory;

    public ConfigurableVideoHostingTests(ErpTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SelfHostedJitsiConfiguration_ReturnsCustomServerUrlAndJoinUrl()
    {
        const string customVpsUrl = "https://telehealth-vps.healthcare.local";

        using var customFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Video:ServerUrl"] = customVpsUrl
                });
            });
        });

        var rootClient = customFactory.CreateClient();
        var patientLogin = await TestAuthHelper.LoginAsync(rootClient, "patient@erp.local", "Password123!");
        var pharmLogin = await TestAuthHelper.LoginAsync(rootClient, "pharmacist@erp.local", "Password123!");

        var patientClient = TestAuthHelper.CreateAuthenticatedClient(_factory, patientLogin.Token);
        var pharmClient = TestAuthHelper.CreateAuthenticatedClient(_factory, pharmLogin.Token);

        // Replace request headers on custom clients with tokens
        var customPatientClient = customFactory.CreateClient();
        customPatientClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", patientLogin.Token);

        var customPharmClient = customFactory.CreateClient();
        customPharmClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", pharmLogin.Token);

        // Create and accept consultation
        var createRes = await customPatientClient.PostAsJsonAsync(
            "/api/telepharmacy/consultations",
            new ConsultationEndpoints.CreateConsultationRequest(
                TelePharmacyDbSeeder.DefaultPharmacistProfileId,
                "Requesting teleconsultation with custom video server testing",
                true),
            TestAuthHelper.JsonOptions);
        createRes.EnsureSuccessStatusCode();

        var created = await createRes.Content.ReadFromJsonAsync<ConsultationEndpoints.ConsultationResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(created);

        var acceptRes = await customPharmClient.PostAsJsonAsync(
            $"/api/telepharmacy/consultations/{created.Id}/accept",
            new ConsultationEndpoints.AcceptConsultationRequest(Fee: 150.00m),
            TestAuthHelper.JsonOptions);
        acceptRes.EnsureSuccessStatusCode();

        var payRes = await customPatientClient.PostAsJsonAsync(
            "/api/core/payments/checkout",
            new PaymentEndpoints.PaymentCheckoutRequest(created.Id, 150.00m),
            TestAuthHelper.JsonOptions);
        payRes.EnsureSuccessStatusCode();

        // Pharmacist starts video
        var startVideoRes = await customPharmClient.PostAsync(
            $"/api/telepharmacy/consultations/{created.Id}/start-video", null);
        Assert.Equal(HttpStatusCode.OK, startVideoRes.StatusCode);

        var video = await startVideoRes.Content.ReadFromJsonAsync<ConsultationEndpoints.StartVideoResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(video);

        // Verify dynamic custom VPS server URL is used
        Assert.Equal(customVpsUrl, video.ServerUrl);
        Assert.StartsWith(customVpsUrl, video.JoinUrl);
        Assert.Equal($"{customVpsUrl}/{video.RoomName}", video.JoinUrl);

        // Verify GetConsultationById also returns the custom server URL
        var detail = await customPatientClient.GetFromJsonAsync<ConsultationEndpoints.ConsultationDetailResponse>(
            $"/api/telepharmacy/consultations/{created.Id}", TestAuthHelper.JsonOptions);
        Assert.NotNull(detail);
        Assert.NotNull(detail.VideoSession);
        Assert.Equal(customVpsUrl, detail.VideoSession.ServerUrl);
        Assert.Equal($"{customVpsUrl}/{video.RoomName}", detail.VideoSession.JoinUrl);
    }
}
