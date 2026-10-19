using Erp.Modules.TelePharmacy.Enums;
using Erp.SharedKernel.Entities;

namespace Erp.Modules.TelePharmacy.Entities;

public class Consultation : BaseEntity
{
    public Guid PatientId { get; private set; }
    public Guid PharmacistProfileId { get; private set; }
    public Guid? TenantId { get; private set; }
    public decimal AgreedFeeAmount { get; private set; }
    public string ChiefComplaintAndSymptoms { get; private set; } = string.Empty;
    public bool ClinicalSummaryConsentGranted { get; private set; }
    public ConsultationStatus Status { get; private set; } = ConsultationStatus.Pending;
    public string? RejectionReason { get; private set; }
    public DateTime RequestedAtUtc { get; private set; } = DateTime.UtcNow;
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime? AcceptedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }

    private Consultation() { }

    public static Consultation Create(
        Guid patientId,
        Guid pharmacistProfileId,
        Guid? tenantId,
        decimal agreedFeeAmount,
        string chiefComplaintAndSymptoms,
        bool clinicalSummaryConsentGranted,
        DateTime expiresAtUtc)
    {
        return new Consultation
        {
            PatientId = patientId,
            PharmacistProfileId = pharmacistProfileId,
            TenantId = tenantId,
            AgreedFeeAmount = agreedFeeAmount,
            ChiefComplaintAndSymptoms = chiefComplaintAndSymptoms,
            ClinicalSummaryConsentGranted = clinicalSummaryConsentGranted,
            ExpiresAtUtc = expiresAtUtc
        };
    }

    public void Accept(decimal? agreedFeeAmount = null)
    {
        if (agreedFeeAmount.HasValue)
            AgreedFeeAmount = agreedFeeAmount.Value;

        Status = ConsultationStatus.AcceptedAwaitingPayment;
        AcceptedAtUtc = DateTime.UtcNow;
    }

    public void Reject(string reason)
    {
        Status = ConsultationStatus.Rejected;
        RejectionReason = reason;
    }

    public void Expire() => Status = ConsultationStatus.Expired;

    public void ActivateChat() => Status = ConsultationStatus.ActiveChat;

    public void StartVideo() => Status = ConsultationStatus.InVideoCall;

    public void ReturnToChat() => Status = ConsultationStatus.ActiveChat;

    public void Complete()
    {
        Status = ConsultationStatus.Completed;
        CompletedAtUtc = DateTime.UtcNow;
    }
}
