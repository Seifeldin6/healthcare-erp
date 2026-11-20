namespace Erp.Modules.TelePharmacy.Endpoints;

public static partial class ConsultationEndpoints
{
    public sealed record CreateConsultationRequest(
        Guid PharmacistProfileId,
        string ChiefComplaintAndSymptoms,
        bool ClinicalSummaryConsentGranted,
        Guid? PatientId = null,
        int? DurationHours = null);

    public sealed record AcceptConsultationRequest(decimal? Fee = null);

    public sealed record RejectConsultationRequest(string RejectionReason);

    public sealed record IssueHerbalPrescriptionRequest(
        string? ClinicalDiagnosisSummary,
        List<PrescribedHerbItem>? Herbs,
        string? PrescribedHerbsJson,
        string? DrugHerbInteractionWarnings,
        string? DietaryAndLifestyleInstructions,
        int DurationDays = 30,
        string? PdfDocumentUrl = null);

    public sealed record PrescribedHerbItem(
        Guid? HerbId,
        string HerbName,
        string Dosage,
        string PreparationInstructions,
        string? Frequency = null,
        int? DurationDays = null);

    public sealed record ConsultationResponse(
        Guid Id,
        Guid PatientId,
        Guid PharmacistProfileId,
        Guid? TenantId,
        decimal AgreedFeeAmount,
        string ChiefComplaintAndSymptoms,
        bool ClinicalSummaryConsentGranted,
        string Status,
        string? RejectionReason,
        DateTime RequestedAtUtc,
        DateTime ExpiresAtUtc,
        DateTime? AcceptedAtUtc,
        DateTime? CompletedAtUtc);

    public sealed record ChatMessageResponse(
        Guid Id,
        Guid ChatRoomId,
        Guid SenderUserId,
        string SenderRole,
        string MessageType,
        string? MessageContent,
        string? AttachmentUrl,
        bool IsRead,
        DateTime SentAtUtc);

    public sealed record ChatRoomDetailResponse(
        Guid Id,
        bool IsReadOnly,
        DateTime OpenedAtUtc,
        DateTime? ClosedAtUtc);

    public sealed record HerbalPrescriptionDetailResponse(
        Guid Id,
        string PrescriptionNumber,
        string? ClinicalDiagnosisSummary,
        string PrescribedHerbsJson,
        string? DrugHerbInteractionWarnings,
        string? DietaryAndLifestyleInstructions,
        int DurationDays,
        string? PdfDocumentUrl,
        DateTime IssuedAtUtc);

    public sealed record VideoSessionDetailResponse(
        Guid VideoSessionId,
        Guid Id,
        string Provider,
        string ServerUrl,
        string RoomName,
        string JoinUrl,
        string ProviderType,
        string ExternalRoomName,
        string SessionStatus,
        DateTime? StartedAtUtc,
        DateTime? EndedAtUtc,
        int? DurationSeconds);

    public sealed record ConsultationDetailResponse(
        ConsultationResponse Consultation,
        Guid? ChatRoomId,
        bool IsChatReadOnly,
        ChatRoomDetailResponse? ChatRoom,
        HerbalPrescriptionDetailResponse? Prescription,
        VideoSessionDetailResponse? VideoSession);

    public sealed record StartVideoResponse(
        Guid VideoSessionId,
        Guid ConsultationId,
        string Provider,
        string ServerUrl,
        string RoomName,
        string JoinUrl,
        string Token,
        string Status,
        DateTime? StartedAtUtc);

    public sealed record EndVideoResponse(
        Guid ConsultationId,
        Guid? VideoSessionId,
        string Status,
        DateTime? EndedAtUtc,
        int? DurationSeconds);

    public sealed record IssueHerbalPrescriptionResponse(
        Guid PrescriptionId,
        string PrescriptionNumber,
        Guid ConsultationId,
        Guid PatientId,
        Guid PharmacistProfileId,
        string? ClinicalDiagnosisSummary,
        string PrescribedHerbsJson,
        string? DrugHerbInteractionWarnings,
        string? DietaryAndLifestyleInstructions,
        int DurationDays,
        string? PdfDocumentUrl,
        DateTime IssuedAtUtc,
        string ConsultationStatus,
        bool ChatRoomClosed);
}
