using System.Net;
using System.Net.Http.Json;
using Erp.Modules.TelePharmacy.Data;
using Erp.Modules.TelePharmacy.Endpoints;
using Erp.SharedKernel.Pagination;
using Erp.Tests.Fixtures;
using Erp.Tests.Helpers;
using Xunit;

namespace Erp.Tests.Integration;

[Collection("IntegrationTests")]
public class HerbsAndPharmacistsTests
{
    private readonly ErpTestWebApplicationFactory _factory;

    public HerbsAndPharmacistsTests(ErpTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetHerbs_AsAuthenticatedUser_ReturnsSeededHerbs()
    {
        var rootClient = _factory.CreateClient();
        var patientLogin = await TestAuthHelper.LoginAsync(rootClient, "patient@erp.local", "Password123!");
        var patientClient = TestAuthHelper.CreateAuthenticatedClient(_factory, patientLogin.Token);

        var response = await patientClient.GetAsync("/api/telepharmacy/herbs?pageNumber=1&pageSize=10");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PagedResult<HerbsEndpoints.HerbResponse>>(TestAuthHelper.JsonOptions);
        Assert.NotNull(result);
        Assert.True(result.TotalCount >= 5);
        Assert.Contains(result.Items, h => h.EnglishCommonName == "Ginger");
        Assert.Contains(result.Items, h => h.EnglishCommonName == "Chamomile");
    }

    [Fact]
    public async Task CreateHerb_AsClinicalStaff_Succeeds()
    {
        var rootClient = _factory.CreateClient();
        var pharmLogin = await TestAuthHelper.LoginAsync(rootClient, "pharmacist@erp.local", "Password123!");
        var pharmClient = TestAuthHelper.CreateAuthenticatedClient(_factory, pharmLogin.Token);

        var uniqueScientific = $"Mentha piperita {Guid.NewGuid():N}";
        var newHerbRequest = new HerbsEndpoints.CreateHerbRequest(
            ScientificName: uniqueScientific,
            ArabicCommonName: "نعناع فلفلي",
            EnglishCommonName: "Peppermint",
            ActiveIngredients: "[\"Menthol\", \"Menthone\"]",
            ContraindicationsJson: "[\"GERD\", \"Hiatal hernia\"]",
            StandardPreparationForms: "Infusion, Essential Oil",
            IsApprovedForPrescription: true);

        var response = await pharmClient.PostAsJsonAsync(
            "/api/telepharmacy/herbs",
            newHerbRequest,
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdHerb = await response.Content.ReadFromJsonAsync<HerbsEndpoints.HerbResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(createdHerb);
        Assert.Equal(uniqueScientific, createdHerb.ScientificName);
        Assert.Equal("Peppermint", createdHerb.EnglishCommonName);
        Assert.True(createdHerb.IsApprovedForPrescription);

        // Verify fetching by ID
        var getByIdRes = await pharmClient.GetAsync($"/api/telepharmacy/herbs/{createdHerb.Id}");
        Assert.Equal(HttpStatusCode.OK, getByIdRes.StatusCode);
    }

    [Fact]
    public async Task GetApprovedAvailablePharmacists_ReturnsSeededPharmacist()
    {
        var rootClient = _factory.CreateClient();
        var patientLogin = await TestAuthHelper.LoginAsync(rootClient, "patient@erp.local", "Password123!");
        var patientClient = TestAuthHelper.CreateAuthenticatedClient(_factory, patientLogin.Token);

        var response = await patientClient.GetAsync("/api/telepharmacy/pharmacists?pageNumber=1&pageSize=10");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PagedResult<PharmacistEndpoints.PharmacistResponse>>(TestAuthHelper.JsonOptions);
        Assert.NotNull(result);
        Assert.True(result.TotalCount >= 1);
        Assert.Contains(result.Items, p => p.Id == TelePharmacyDbSeeder.DefaultPharmacistProfileId);
    }

    [Fact]
    public async Task UpdatePharmacistProfile_And_VerifyToggle_Succeeds()
    {
        var rootClient = _factory.CreateClient();
        var pharmLogin = await TestAuthHelper.LoginAsync(rootClient, "pharmacist@erp.local", "Password123!");
        var pharmClient = TestAuthHelper.CreateAuthenticatedClient(_factory, pharmLogin.Token);

        // 1. Update Profile Details
        var updateRequest = new PharmacistEndpoints.UpsertPharmacistProfileRequest(
            Bio: "Updated bio for clinical phytotherapy consultations.",
            ConsultationFee: 175.00m);

        var updateRes = await pharmClient.PutAsJsonAsync(
            "/api/telepharmacy/pharmacists/me/profile",
            updateRequest,
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, updateRes.StatusCode);
        var updatedProfile = await updateRes.Content.ReadFromJsonAsync<PharmacistEndpoints.PharmacistResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(updatedProfile);
        Assert.Equal("Updated bio for clinical phytotherapy consultations.", updatedProfile.Bio);
        Assert.Equal(175.00m, updatedProfile.ConsultationFee);

        // 2. Toggle Availability
        var availRes = await pharmClient.PutAsJsonAsync(
            "/api/telepharmacy/pharmacists/me/availability",
            new PharmacistEndpoints.UpdateAvailabilityRequest(IsAvailable: false),
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, availRes.StatusCode);
        var availResult = await availRes.Content.ReadFromJsonAsync<PharmacistEndpoints.PharmacistAvailabilityResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(availResult);
        Assert.False(availResult.IsAvailableForRequests);

        // Reset availability back to true
        var resetAvailRes = await pharmClient.PutAsJsonAsync(
            "/api/telepharmacy/pharmacists/me/availability",
            new PharmacistEndpoints.UpdateAvailabilityRequest(IsAvailable: true),
            TestAuthHelper.JsonOptions);
        Assert.Equal(HttpStatusCode.OK, resetAvailRes.StatusCode);

        // 3. Verify Pharmacist Verification endpoint
        var verifyRes = await pharmClient.PostAsJsonAsync(
            $"/api/telepharmacy/pharmacists/{TelePharmacyDbSeeder.DefaultPharmacistProfileId}/verify",
            new PharmacistEndpoints.VerifyPharmacistRequest(Approve: true),
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, verifyRes.StatusCode);
        var verifiedProfile = await verifyRes.Content.ReadFromJsonAsync<PharmacistEndpoints.PharmacistResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(verifiedProfile);
        Assert.Equal("Approved", verifiedProfile.VerificationStatus);

        // Reset profile fee and bio back to default
        await pharmClient.PutAsJsonAsync(
            "/api/telepharmacy/pharmacists/me/profile",
            new PharmacistEndpoints.UpsertPharmacistProfileRequest(
                Bio: "Licensed clinical pharmacist specializing in herbal pharmacology, natural remedies, and drug-herb interactions.",
                ConsultationFee: 150.00m),
            TestAuthHelper.JsonOptions);
    }
}
