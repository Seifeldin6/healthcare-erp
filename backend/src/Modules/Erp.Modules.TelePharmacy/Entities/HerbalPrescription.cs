using Erp.SharedKernel.Entities;

namespace Erp.Modules.TelePharmacy.Entities;

public class HerbalPrescription : BaseEntity
{
    public Guid ConsultationId { get; private set; }
    public Guid PatientId { get; private set; }
    public Guid PharmacistProfileId { get; private set; }
    public string PrescriptionNumber { get; private set; } = string.Empty;
    public string? ClinicalDiagnosisSummary { get; private set; }
    public string PrescribedHerbsJson { get; private set; } = "[]";
    public string? DrugHerbInteractionWarnings { get; private set; }
    public string? DietaryAndLifestyleInstructions { get; private set; }
    public int DurationDays { get; private set; }
    public string? PdfDocumentUrl { get; private set; }
    public DateTime IssuedAtUtc { get; private set; } = DateTime.UtcNow;

    private HerbalPrescription() { }

    public static HerbalPrescription Create(
        Guid consultationId,
        Guid patientId,
        Guid pharmacistProfileId,
        string prescriptionNumber,
        string? clinicalDiagnosisSummary,
        string prescribedHerbsJson,
        string? drugHerbInteractionWarnings,
        string? dietaryAndLifestyleInstructions,
        int durationDays,
        string? pdfDocumentUrl)
    {
        return new HerbalPrescription
        {
            ConsultationId = consultationId,
            PatientId = patientId,
            PharmacistProfileId = pharmacistProfileId,
            PrescriptionNumber = prescriptionNumber,
            ClinicalDiagnosisSummary = clinicalDiagnosisSummary,
            PrescribedHerbsJson = prescribedHerbsJson,
            DrugHerbInteractionWarnings = drugHerbInteractionWarnings,
            DietaryAndLifestyleInstructions = dietaryAndLifestyleInstructions,
            DurationDays = durationDays,
            PdfDocumentUrl = pdfDocumentUrl
        };
    }

    public void AttachPdf(string pdfUrl) => PdfDocumentUrl = pdfUrl;
}
