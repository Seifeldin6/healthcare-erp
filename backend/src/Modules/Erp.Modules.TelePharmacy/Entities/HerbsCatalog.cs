using Erp.SharedKernel.Entities;

namespace Erp.Modules.TelePharmacy.Entities;

public class HerbsCatalog : BaseEntity
{
    public string ScientificName { get; private set; } = string.Empty;
    public string ArabicCommonName { get; private set; } = string.Empty;
    public string EnglishCommonName { get; private set; } = string.Empty;
    public string? ActiveIngredients { get; private set; }
    public string ContraindicationsJson { get; private set; } = "[]";
    public string? StandardPreparationForms { get; private set; }
    public bool IsApprovedForPrescription { get; private set; }

    private HerbsCatalog() { }

    public static HerbsCatalog Create(
        string scientificName,
        string arabicCommonName,
        string englishCommonName,
        string? activeIngredients,
        string contraindicationsJson,
        string? standardPreparationForms,
        bool isApprovedForPrescription)
    {
        return new HerbsCatalog
        {
            ScientificName = scientificName,
            ArabicCommonName = arabicCommonName,
            EnglishCommonName = englishCommonName,
            ActiveIngredients = activeIngredients,
            ContraindicationsJson = contraindicationsJson,
            StandardPreparationForms = standardPreparationForms,
            IsApprovedForPrescription = isApprovedForPrescription
        };
    }
}
