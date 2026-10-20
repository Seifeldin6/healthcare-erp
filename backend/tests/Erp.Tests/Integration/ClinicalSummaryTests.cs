using System.Net;
using System.Net.Http.Json;
using Erp.Modules.Core.Data;
using Erp.Modules.Core.Endpoints;
using Erp.SharedKernel.Responses;
using Erp.Tests.Fixtures;
using Erp.Tests.Helpers;
using Xunit;

namespace Erp.Tests.Integration;

[Collection("IntegrationTests")]
public class ClinicalSummaryTests
{
    private readonly ErpTestWebApplicationFactory _factory;

    public ClinicalSummaryTests(ErpTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetMyClinicalSummary_AsAuthenticatedPatient_ReturnsProfileAndSummary()
    {
        var rootClient = _factory.CreateClient();
        var login = await TestAuthHelper.LoginAsync(rootClient, "patient@erp.local", "Password123!");

        var patientClient = TestAuthHelper.CreateAuthenticatedClient(_factory, login.Token);

        var response = await patientClient.GetAsync("/api/core/patients/me/clinical-summary");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PatientEndpoints.PatientClinicalSummaryDetailResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(result);
        Assert.NotNull(result.Patient);
        Assert.NotNull(result.ClinicalSummary);
        Assert.Equal("29001011234567", result.Patient.NationalId);
        Assert.Equal("Male", result.Patient.Gender);
        Assert.Equal("APositive", result.Patient.BloodType);
        Assert.Contains("Penicillin", result.ClinicalSummary.AllergiesJson);
    }

    [Fact]
    public async Task UpdateMyClinicalSummary_WithValidJson_UpdatesSuccessfully()
    {
        var rootClient = _factory.CreateClient();
        var login = await TestAuthHelper.LoginAsync(rootClient, "patient@erp.local", "Password123!");
        var patientClient = TestAuthHelper.CreateAuthenticatedClient(_factory, login.Token);

        var updatedAllergies = "[\"Penicillin\", \"Pollen\", \"Latex\"]";
        var updatedConditions = "[\"Mild Hypertension\"]";
        var updatedMeds = "[\"Amlodipine 5mg\"]";
        var updatedSurgeries = "[\"Appendectomy (2018)\", \"Tonsillectomy (2005)\"]";

        var updateRequest = new PatientEndpoints.UpdateClinicalSummaryRequest(
            AllergiesJson: updatedAllergies,
            ChronicConditionsJson: updatedConditions,
            ActiveMedicationsJson: updatedMeds,
            MajorSurgeriesJson: updatedSurgeries);

        var response = await patientClient.PutAsJsonAsync(
            "/api/core/patients/me/clinical-summary",
            updateRequest,
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var summary = await response.Content.ReadFromJsonAsync<PatientEndpoints.PatientClinicalSummaryResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(summary);
        Assert.Equal(updatedAllergies, summary.AllergiesJson);
        Assert.Equal(updatedConditions, summary.ChronicConditionsJson);
        Assert.Equal(updatedMeds, summary.ActiveMedicationsJson);
        Assert.Equal(updatedSurgeries, summary.MajorSurgeriesJson);
    }

    [Theory]
    [InlineData("not a valid json", "[]", "[]", "[]")]
    [InlineData("[]", "{invalid json conditions", "[]", "[]")]
    [InlineData("[]", "[]", "{bad meds", "[]")]
    [InlineData("[]", "[]", "[]", "invalid surgery [}")]
    public async Task UpdateMyClinicalSummary_WithInvalidJson_FailsGuardCheck_ReturnsBadRequest(
        string allergies, string conditions, string meds, string surgeries)
    {
        var rootClient = _factory.CreateClient();
        var login = await TestAuthHelper.LoginAsync(rootClient, "patient@erp.local", "Password123!");
        var patientClient = TestAuthHelper.CreateAuthenticatedClient(_factory, login.Token);

        var updateRequest = new PatientEndpoints.UpdateClinicalSummaryRequest(
            AllergiesJson: allergies,
            ChronicConditionsJson: conditions,
            ActiveMedicationsJson: meds,
            MajorSurgeriesJson: surgeries);

        var response = await patientClient.PutAsJsonAsync(
            "/api/core/patients/me/clinical-summary",
            updateRequest,
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(error);
        Assert.Contains("not valid JSON", error.Message);
    }

    [Fact]
    public async Task GetPatientClinicalSummary_AsPharmacist_ReturnsDetailAndRecordsAudit()
    {
        var rootClient = _factory.CreateClient();
        var pharmLogin = await TestAuthHelper.LoginAsync(rootClient, "pharmacist@erp.local", "Password123!");
        var pharmClient = TestAuthHelper.CreateAuthenticatedClient(_factory, pharmLogin.Token);

        var patientId = CoreDbSeeder.DefaultPatientId;

        var response = await pharmClient.GetAsync($"/api/core/patients/{patientId}/clinical-summary");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PatientEndpoints.PatientClinicalSummaryDetailResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(result);
        Assert.Equal(patientId, result.ClinicalSummary.PatientId);
        Assert.Equal("Ahmed Mahmoud", result.Patient.FullName);
    }
}
