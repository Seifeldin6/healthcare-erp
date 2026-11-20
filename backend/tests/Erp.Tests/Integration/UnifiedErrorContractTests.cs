using System.Net;
using System.Net.Http.Json;
using System.Text;
using Erp.Modules.Core.Data;
using Erp.Modules.Core.Endpoints;
using Erp.Modules.TelePharmacy.Data;
using Erp.Modules.TelePharmacy.Endpoints;
using Erp.SharedKernel.Pagination;
using Erp.SharedKernel.Responses;
using Erp.Tests.Fixtures;
using Erp.Tests.Helpers;
using Xunit;

namespace Erp.Tests.Integration;

[Collection("IntegrationTests")]
public class UnifiedErrorContractTests
{
    private readonly ErpTestWebApplicationFactory _factory;

    public UnifiedErrorContractTests(ErpTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Login_InvalidCredentials_Returns401_WithStandardizedErrorCode()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/core/auth/login",
            new { EmailOrPhone = "nobody@erp.local", Password = "wrong" },
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        var err = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(err);
        Assert.False(err.Success);
        Assert.Equal(401, err.StatusCode);
        Assert.Equal("INVALID_CREDENTIALS", err.Code);
        Assert.Equal("Invalid email or password.", err.Message);
    }

    [Fact]
    public async Task ProtectedRoute_WithoutToken_Returns401_WithSessionExpiredCode()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/telepharmacy/herbs?pageNumber=1&pageSize=10");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        var err = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(err);
        Assert.False(err.Success);
        Assert.Equal(401, err.StatusCode);
        Assert.Equal("SESSION_EXPIRED", err.Code);
        Assert.Equal("Your session has expired. Please sign in again.", err.Message);
    }

    [Fact]
    public async Task RegisterPatient_WithMissingRequiredFields_Returns400_WithValidationErrorCode()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            "/api/core/auth/register-patient",
            new StringContent("{}", Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        var err = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(err);
        Assert.False(err.Success);
        Assert.Equal(400, err.StatusCode);
        Assert.Equal("VALIDATION_ERROR", err.Code);
        Assert.False(string.IsNullOrWhiteSpace(err.Message));
    }

    [Fact]
    public async Task CreateHerb_WithBlankScientificName_Returns400_WithValidationErrorCode()
    {
        var rootClient = _factory.CreateClient();
        var pharmLogin = await TestAuthHelper.LoginAsync(rootClient, "pharmacist@erp.local", "Password123!");
        var pharmClient = TestAuthHelper.CreateAuthenticatedClient(_factory, pharmLogin.Token);

        var badRequest = new { ScientificName = "", ArabicCommonName = "نعناع", EnglishCommonName = "", IsApprovedForPrescription = true };

        var response = await pharmClient.PostAsJsonAsync(
            "/api/telepharmacy/herbs",
            badRequest,
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        var err = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(err);
        Assert.False(err.Success);
        Assert.Equal(400, err.StatusCode);
        Assert.Equal("VALIDATION_ERROR", err.Code);
        Assert.Equal("Please provide the scientific name, English common name, and Arabic common name.", err.Message);
    }

    [Fact]
    public async Task GetHerbById_WithUnknownId_Returns404_WithHerbNotFoundCode()
    {
        var rootClient = _factory.CreateClient();
        var pharmLogin = await TestAuthHelper.LoginAsync(rootClient, "pharmacist@erp.local", "Password123!");
        var pharmClient = TestAuthHelper.CreateAuthenticatedClient(_factory, pharmLogin.Token);

        var response = await pharmClient.GetAsync($"/api/telepharmacy/herbs/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        var err = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(err);
        Assert.False(err.Success);
        Assert.Equal(404, err.StatusCode);
        Assert.Equal("HERB_NOT_FOUND", err.Code);
        Assert.Equal("The requested herb could not be found.", err.Message);
    }

    [Fact]
    public async Task RegisterPatient_DuplicateEmail_Returns409_WithEmailAlreadyExistsCode()
    {
        var client = _factory.CreateClient();

        var payload = """
        {
          "email": "patient@erp.local",
          "phoneNumber": "+201099999991",
          "password": "Password123!",
          "fullName": "Duplicate",
          "nationalId": "29111111111111",
          "dateOfBirth": "1990-01-01",
          "gender": "Male",
          "bloodType": "APositive",
          "weightKg": 70.0,
          "heightCm": 170.0
        }
        """;

        var response = await client.PostAsync(
            "/api/core/auth/register-patient",
            new StringContent(payload, Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        var err = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(err);
        Assert.False(err.Success);
        Assert.Equal(409, err.StatusCode);
        Assert.Equal("EMAIL_ALREADY_EXISTS", err.Code);
        Assert.Equal("This email address is already registered.", err.Message);
        Assert.Null(err.Errors);
    }

    [Fact]
    public async Task ErrorResponse_Shape_HasAllRequiredFields()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/core/auth/login",
            new { EmailOrPhone = "ghost@erp.local", Password = "wrong" },
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var err = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(err);

        Assert.False(err.Success);
        Assert.Equal(401, err.StatusCode);
        Assert.Equal("INVALID_CREDENTIALS", err.Code);
        Assert.Equal("Invalid email or password.", err.Message);
        Assert.False(string.IsNullOrWhiteSpace(err.TraceId));
    }

    [Fact]
    public async Task GetHerbs_WithExcessivePageSize_EnforcesHardCapOf100()
    {
        var rootClient = _factory.CreateClient();
        var patientLogin = await TestAuthHelper.LoginAsync(rootClient, "patient@erp.local", "Password123!");
        var patientClient = TestAuthHelper.CreateAuthenticatedClient(_factory, patientLogin.Token);

        var response = await patientClient.GetAsync("/api/telepharmacy/herbs?pageNumber=1&pageSize=500");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PagedResult<HerbsEndpoints.HerbResponse>>(TestAuthHelper.JsonOptions);
        Assert.NotNull(result);
        Assert.True(result.PageSize <= 100);
        Assert.True(result.Items.Count <= 100);
    }

    [Fact]
    public async Task GetConsultationById_PatientAccessingAnotherPatientConsultation_Returns403Forbidden()
    {
        var rootClient = _factory.CreateClient();

        // 1. Register a distinct patient B
        var regPayload = $$"""
        {
          "email": "patient.b.{{Guid.NewGuid():N}}@erp.local",
          "phoneNumber": "+2010{{Random.Shared.Next(10000000, 99999999)}}",
          "password": "Password123!",
          "fullName": "Patient B",
          "nationalId": "29{{Random.Shared.Next(10000000, 99999999)}}1234",
          "dateOfBirth": "1992-05-10",
          "gender": "Female",
          "bloodType": "BPositive",
          "weightKg": 65.0,
          "heightCm": 165.0
        }
        """;

        var regRes = await rootClient.PostAsync(
            "/api/core/auth/register-patient",
            new StringContent(regPayload, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.OK, regRes.StatusCode);
        var patientB = await regRes.Content.ReadFromJsonAsync<AuthEndpoints.RegisterPatientResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(patientB);

        var patientBClient = TestAuthHelper.CreateAuthenticatedClient(_factory, patientB.Token);

        // 2. Patient B creates a consultation
        var createRequest = new ConsultationEndpoints.CreateConsultationRequest(
            PharmacistProfileId: TelePharmacyDbSeeder.DefaultPharmacistProfileId,
            ChiefComplaintAndSymptoms: "Confidential symptoms of Patient B",
            ClinicalSummaryConsentGranted: true);

        var createRes = await patientBClient.PostAsJsonAsync(
            "/api/telepharmacy/consultations",
            createRequest,
            TestAuthHelper.JsonOptions);
        Assert.Equal(HttpStatusCode.Created, createRes.StatusCode);
        var consultation = await createRes.Content.ReadFromJsonAsync<ConsultationEndpoints.ConsultationResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(consultation);

        // 3. Patient A (seeded) attempts to access Patient B's consultation
        var patientALogin = await TestAuthHelper.LoginAsync(rootClient, "patient@erp.local", "Password123!");
        var patientAClient = TestAuthHelper.CreateAuthenticatedClient(_factory, patientALogin.Token);

        var idorRes = await patientAClient.GetAsync($"/api/telepharmacy/consultations/{consultation.Id}");
        Assert.Equal(HttpStatusCode.Forbidden, idorRes.StatusCode);

        var err = await idorRes.Content.ReadFromJsonAsync<ErrorResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(err);
        Assert.False(err.Success);
        Assert.Equal(403, err.StatusCode);
        Assert.Equal("FORBIDDEN", err.Code);
    }

    [Fact]
    public async Task Checkout_PatientPayingForAnotherPatientConsultation_Returns403Forbidden()
    {
        var rootClient = _factory.CreateClient();

        // 1. Register Patient C
        var regPayload = $$"""
        {
          "email": "patient.c.{{Guid.NewGuid():N}}@erp.local",
          "phoneNumber": "+2010{{Random.Shared.Next(10000000, 99999999)}}",
          "password": "Password123!",
          "fullName": "Patient C",
          "nationalId": "29{{Random.Shared.Next(10000000, 99999999)}}5678",
          "dateOfBirth": "1994-08-15",
          "gender": "Male",
          "bloodType": "OPositive",
          "weightKg": 75.0,
          "heightCm": 178.0
        }
        """;

        var regRes = await rootClient.PostAsync(
            "/api/core/auth/register-patient",
            new StringContent(regPayload, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.OK, regRes.StatusCode);
        var patientC = await regRes.Content.ReadFromJsonAsync<AuthEndpoints.RegisterPatientResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(patientC);

        var patientCClient = TestAuthHelper.CreateAuthenticatedClient(_factory, patientC.Token);

        // 2. Patient C creates a consultation
        var createRequest = new ConsultationEndpoints.CreateConsultationRequest(
            PharmacistProfileId: TelePharmacyDbSeeder.DefaultPharmacistProfileId,
            ChiefComplaintAndSymptoms: "Consultation of Patient C",
            ClinicalSummaryConsentGranted: true);

        var createRes = await patientCClient.PostAsJsonAsync(
            "/api/telepharmacy/consultations",
            createRequest,
            TestAuthHelper.JsonOptions);
        Assert.Equal(HttpStatusCode.Created, createRes.StatusCode);
        var consultation = await createRes.Content.ReadFromJsonAsync<ConsultationEndpoints.ConsultationResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(consultation);

        // Pharmacist accepts it
        var pharmLogin = await TestAuthHelper.LoginAsync(rootClient, "pharmacist@erp.local", "Password123!");
        var pharmClient = TestAuthHelper.CreateAuthenticatedClient(_factory, pharmLogin.Token);
        var acceptRes = await pharmClient.PostAsJsonAsync(
            $"/api/telepharmacy/consultations/{consultation.Id}/accept",
            new ConsultationEndpoints.AcceptConsultationRequest(Fee: 150.00m),
            TestAuthHelper.JsonOptions);
        Assert.Equal(HttpStatusCode.OK, acceptRes.StatusCode);

        // 3. Patient A attempts to pay for Patient C's consultation
        var patientALogin = await TestAuthHelper.LoginAsync(rootClient, "patient@erp.local", "Password123!");
        var patientAClient = TestAuthHelper.CreateAuthenticatedClient(_factory, patientALogin.Token);

        var payRes = await patientAClient.PostAsJsonAsync(
            "/api/core/payments/checkout",
            new PaymentEndpoints.PaymentCheckoutRequest(consultation.Id, 150.00m),
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.Forbidden, payRes.StatusCode);
        var err = await payRes.Content.ReadFromJsonAsync<ErrorResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(err);
        Assert.False(err.Success);
        Assert.Equal(403, err.StatusCode);
        Assert.Equal("FORBIDDEN", err.Code);
    }
}
