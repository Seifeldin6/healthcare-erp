using System.Net;
using System.Net.Http.Json;
using Erp.Modules.Core.Data;
using Erp.Modules.Core.Endpoints;
using Erp.Modules.TelePharmacy.Data;
using Erp.Modules.TelePharmacy.Endpoints;
using Erp.SharedKernel.Responses;
using Erp.Tests.Fixtures;
using Erp.Tests.Helpers;
using Xunit;

namespace Erp.Tests.Integration;

[Collection("IntegrationTests")]
public class ConsultationLifecycleTests
{
    private readonly ErpTestWebApplicationFactory _factory;

    public ConsultationLifecycleTests(ErpTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task FullConsultationLifecycle_StateMachine_ExecutesCorrectly()
    {
        var rootClient = _factory.CreateClient();

        // 0. Log in Patient and Pharmacist
        var patientLogin = await TestAuthHelper.LoginAsync(rootClient, "patient@erp.local", "Password123!");
        var pharmLogin = await TestAuthHelper.LoginAsync(rootClient, "pharmacist@erp.local", "Password123!");

        var patientClient = TestAuthHelper.CreateAuthenticatedClient(_factory, patientLogin.Token);
        var pharmClient = TestAuthHelper.CreateAuthenticatedClient(_factory, pharmLogin.Token);

        var pharmacistProfileId = TelePharmacyDbSeeder.DefaultPharmacistProfileId;

        // Step a: Patient requests consultation (Initial Status: Pending)
        var createRequest = new ConsultationEndpoints.CreateConsultationRequest(
            PharmacistProfileId: pharmacistProfileId,
            ChiefComplaintAndSymptoms: "Severe migraine attacks and sleep disturbances for the past 2 weeks.",
            ClinicalSummaryConsentGranted: true,
            DurationHours: 24);

        var createRes = await patientClient.PostAsJsonAsync(
            "/api/telepharmacy/consultations",
            createRequest,
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.Created, createRes.StatusCode);

        var created = await createRes.Content.ReadFromJsonAsync<ConsultationEndpoints.ConsultationResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal("Pending", created.Status);
        Assert.Equal(150.00m, created.AgreedFeeAmount);

        var consultationId = created.Id;

        // Verify guard check: Patient payment MUST fail if status is 'Pending' (not 'AcceptedAwaitingPayment')
        var prematurePayRes = await patientClient.PostAsJsonAsync(
            "/api/core/payments/checkout",
            new PaymentEndpoints.PaymentCheckoutRequest(consultationId, 150.00m),
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.Conflict, prematurePayRes.StatusCode);
        var prematureErr = await prematurePayRes.Content.ReadFromJsonAsync<ErrorResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(prematureErr);
        Assert.Contains("AcceptedAwaitingPayment", prematureErr.Message);

        // Step b: Pharmacist accepts consultation (Status -> AcceptedAwaitingPayment + AgreedFee snapshot)
        var acceptRes = await pharmClient.PostAsJsonAsync(
            $"/api/telepharmacy/consultations/{consultationId}/accept",
            new ConsultationEndpoints.AcceptConsultationRequest(Fee: 150.00m),
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, acceptRes.StatusCode);
        var accepted = await acceptRes.Content.ReadFromJsonAsync<ConsultationEndpoints.ConsultationResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(accepted);
        Assert.Equal("AcceptedAwaitingPayment", accepted.Status);
        Assert.Equal(150.00m, accepted.AgreedFeeAmount);
        Assert.NotNull(accepted.AcceptedAtUtc);

        // Step c: Patient pays via /api/core/payments/checkout
        // Verifies ConsultationPaidHandler transitions status to ActiveChat and creates ChatRoom idempotently
        var payRes = await patientClient.PostAsJsonAsync(
            "/api/core/payments/checkout",
            new PaymentEndpoints.PaymentCheckoutRequest(consultationId, 150.00m),
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, payRes.StatusCode);
        var payResult = await payRes.Content.ReadFromJsonAsync<PaymentEndpoints.PaymentCheckoutResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(payResult);
        Assert.NotEqual(Guid.Empty, payResult.PaymentId);
        Assert.Equal("Paid", payResult.Status);

        // Verify consultation state is now ActiveChat and ChatRoom exists
        var detailRes = await patientClient.GetAsync($"/api/telepharmacy/consultations/{consultationId}");
        Assert.Equal(HttpStatusCode.OK, detailRes.StatusCode);
        var detail = await detailRes.Content.ReadFromJsonAsync<ConsultationEndpoints.ConsultationDetailResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(detail);
        Assert.Equal("ActiveChat", detail.Consultation.Status);
        Assert.NotNull(detail.ChatRoomId);
        Assert.NotNull(detail.ChatRoom);
        Assert.False(detail.IsChatReadOnly);

        var chatRoomId = detail.ChatRoomId.Value;

        // Verify Idempotent payment handling (second payment attempt fails safely without corrupting chat room)
        var repeatPayRes = await patientClient.PostAsJsonAsync(
            "/api/core/payments/checkout",
            new PaymentEndpoints.PaymentCheckoutRequest(consultationId, 150.00m),
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.Conflict, repeatPayRes.StatusCode);

        // Step d: Pharmacist starts Jitsi video call (/start-video -> InVideoCall)
        // Verifying dynamic ServerUrl and JoinUrl
        var startVideoRes = await pharmClient.PostAsync(
            $"/api/telepharmacy/consultations/{consultationId}/start-video", null);

        Assert.Equal(HttpStatusCode.OK, startVideoRes.StatusCode);
        var videoPayload = await startVideoRes.Content.ReadFromJsonAsync<ConsultationEndpoints.StartVideoResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(videoPayload);
        Assert.Equal("Active", videoPayload.Status);
        Assert.Equal(_factory.VideoServerUrl, videoPayload.ServerUrl);
        Assert.StartsWith(_factory.VideoServerUrl, videoPayload.JoinUrl);
        Assert.Contains(videoPayload.RoomName, videoPayload.JoinUrl);
        Assert.False(string.IsNullOrWhiteSpace(videoPayload.Token));

        // Verify consultation status is now InVideoCall
        var inVideoConsultation = await pharmClient.GetFromJsonAsync<ConsultationEndpoints.ConsultationDetailResponse>(
            $"/api/telepharmacy/consultations/{consultationId}", TestAuthHelper.JsonOptions);
        Assert.NotNull(inVideoConsultation);
        Assert.Equal("InVideoCall", inVideoConsultation.Consultation.Status);

        // Pharmacist ends video call (/end-video -> returns status to ActiveChat)
        var endVideoRes = await pharmClient.PostAsync(
            $"/api/telepharmacy/consultations/{consultationId}/end-video", null);

        Assert.Equal(HttpStatusCode.OK, endVideoRes.StatusCode);
        var endVideoResult = await endVideoRes.Content.ReadFromJsonAsync<ConsultationEndpoints.EndVideoResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(endVideoResult);
        Assert.Equal("ActiveChat", endVideoResult.Status);

        // Step e: Pharmacist issues herbal prescription (/herbal-prescription)
        // Transitions consultation to Completed, sets ChatRoom.IsReadOnly = true,
        // and publishes integration event updating PatientClinicalSummary.ActiveHerbalPrescriptionsJson in Core
        var herbPrescriptionRequest = new ConsultationEndpoints.IssueHerbalPrescriptionRequest(
            ClinicalDiagnosisSummary: "Tension cephalalgia with secondary insomnia.",
            Herbs: new List<ConsultationEndpoints.PrescribedHerbItem>
            {
                new(
                    HerbId: null,
                    HerbName: "Matricaria chamomilla (Chamomile)",
                    Dosage: "2g dried flower infusion",
                    PreparationInstructions: "Steep in 200ml boiling water covered for 10 minutes, drink before bedtime",
                    Frequency: "Once daily at night",
                    DurationDays: 14)
            },
            PrescribedHerbsJson: null,
            DrugHerbInteractionWarnings: "No significant adverse interaction with patient's Amlodipine regimen.",
            DietaryAndLifestyleInstructions: "Avoid screen exposure 1 hour before sleep and reduce caffeine intake.",
            DurationDays: 14,
            PdfDocumentUrl: "https://storage.greenlife.local/rx/rx-sample-01.pdf");

        var rxRes = await pharmClient.PostAsJsonAsync(
            $"/api/telepharmacy/consultations/{consultationId}/herbal-prescription",
            herbPrescriptionRequest,
            TestAuthHelper.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, rxRes.StatusCode);
        var rxResult = await rxRes.Content.ReadFromJsonAsync<ConsultationEndpoints.IssueHerbalPrescriptionResponse>(TestAuthHelper.JsonOptions);
        Assert.NotNull(rxResult);
        Assert.Equal("Completed", rxResult.ConsultationStatus);
        Assert.True(rxResult.ChatRoomClosed);
        Assert.StartsWith("RX-", rxResult.PrescriptionNumber);

        // Verify consultation and chat room state in TelePharmacy
        var finalConsultation = await patientClient.GetFromJsonAsync<ConsultationEndpoints.ConsultationDetailResponse>(
            $"/api/telepharmacy/consultations/{consultationId}", TestAuthHelper.JsonOptions);
        Assert.NotNull(finalConsultation);
        Assert.Equal("Completed", finalConsultation.Consultation.Status);
        Assert.True(finalConsultation.IsChatReadOnly);
        Assert.NotNull(finalConsultation.Prescription);
        Assert.Equal(rxResult.PrescriptionNumber, finalConsultation.Prescription.PrescriptionNumber);

        // Verify Core Module was updated via HerbalPrescriptionIssuedHandler
        var patientSummaryRes = await patientClient.GetFromJsonAsync<PatientEndpoints.PatientClinicalSummaryDetailResponse>(
            "/api/core/patients/me/clinical-summary", TestAuthHelper.JsonOptions);
        Assert.NotNull(patientSummaryRes);
        Assert.Contains(rxResult.PrescriptionNumber, patientSummaryRes.ClinicalSummary.ActiveHerbalPrescriptionsJson);
        Assert.Contains("Matricaria chamomilla", patientSummaryRes.ClinicalSummary.ActiveHerbalPrescriptionsJson);
    }
}
