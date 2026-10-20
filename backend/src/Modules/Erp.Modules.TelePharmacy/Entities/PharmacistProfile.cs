using Erp.Modules.TelePharmacy.Enums;
using Erp.SharedKernel.Entities;

namespace Erp.Modules.TelePharmacy.Entities;

public class PharmacistProfile : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid? TenantId { get; private set; }
    public string SyndicateLicenseNumber { get; private set; } = string.Empty;
    public string? LicenseDocumentUrl { get; private set; }
    public VerificationStatus VerificationStatus { get; private set; } = VerificationStatus.PendingReview;
    public PharmacistSpecialization Specialization { get; private set; }
    public string? Bio { get; private set; }
    public decimal ConsultationFee { get; private set; }
    public bool IsAvailableForRequests { get; private set; }
    public DateTime? VerifiedAtUtc { get; private set; }

    private PharmacistProfile() { }

    public static PharmacistProfile Create(
        Guid userId,
        Guid? tenantId,
        string syndicateLicenseNumber,
        string? licenseDocumentUrl,
        PharmacistSpecialization specialization,
        string? bio,
        decimal consultationFee)
    {
        return new PharmacistProfile
        {
            UserId = userId,
            TenantId = tenantId,
            SyndicateLicenseNumber = syndicateLicenseNumber,
            LicenseDocumentUrl = licenseDocumentUrl,
            Specialization = specialization,
            Bio = bio,
            ConsultationFee = consultationFee
        };
    }

    public static PharmacistProfile Create(
        Guid id,
        Guid userId,
        Guid? tenantId,
        string syndicateLicenseNumber,
        string? licenseDocumentUrl,
        PharmacistSpecialization specialization,
        string? bio,
        decimal consultationFee)
    {
        var profile = Create(userId, tenantId, syndicateLicenseNumber, licenseDocumentUrl, specialization, bio, consultationFee);
        profile.Id = id;
        return profile;
    }

    public void Approve()
    {
        VerificationStatus = VerificationStatus.Approved;
        VerifiedAtUtc = DateTime.UtcNow;
    }

    public void Reject() => VerificationStatus = VerificationStatus.Rejected;

    public void Suspend() => VerificationStatus = VerificationStatus.Suspended;

    public void SetAvailability(bool isAvailable) => IsAvailableForRequests = isAvailable;

    public void UpdateDetails(
        string? bio,
        decimal consultationFee,
        PharmacistSpecialization specialization,
        string? licenseDocumentUrl = null)
    {
        Bio = bio;
        ConsultationFee = consultationFee;
        Specialization = specialization;
        if (!string.IsNullOrEmpty(licenseDocumentUrl))
            LicenseDocumentUrl = licenseDocumentUrl;
    }
}
