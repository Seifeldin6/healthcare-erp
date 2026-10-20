using Erp.Modules.TelePharmacy.Entities;
using Erp.Modules.TelePharmacy.Enums;
using Microsoft.EntityFrameworkCore;

namespace Erp.Modules.TelePharmacy.Data;

public static class TelePharmacyDbSeeder
{
    public static readonly Guid DefaultTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid DefaultPharmacistUserId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid DefaultPharmacistProfileId = Guid.Parse("55555555-5555-5555-5555-555555555555");

    public static async Task SeedAsync(TelePharmacyDbContext db, CancellationToken ct = default)
    {
        // 1. Seed approved PharmacistProfile
        if (!await db.PharmacistProfiles.AnyAsync(p => p.UserId == DefaultPharmacistUserId, ct))
        {
            var profile = PharmacistProfile.Create(
                DefaultPharmacistProfileId,
                DefaultPharmacistUserId,
                DefaultTenantId,
                syndicateLicenseNumber: "PHARM-EG-2026-9876",
                licenseDocumentUrl: "https://storage.greenlife.local/licenses/pharm-9876.pdf",
                specialization: PharmacistSpecialization.HerbalAndComplementaryMedicine,
                bio: "Licensed clinical pharmacist specializing in herbal pharmacology, natural remedies, and drug-herb interactions.",
                consultationFee: 150.00m);

            profile.Approve();
            profile.SetAvailability(true);

            db.PharmacistProfiles.Add(profile);
            await db.SaveChangesAsync(ct);
        }

        // 2. Seed 5 realistic HerbsCatalog entries
        var herbsToSeed = new[]
        {
            HerbsCatalog.Create(
                scientificName: "Zingiber officinale",
                arabicCommonName: "زنجبيل",
                englishCommonName: "Ginger",
                activeIngredients: "[\"6-Gingerol\", \"6-Shogaol\", \"Zingiberene\"]",
                contraindicationsJson: "[\"Gallstones\", \"Concurrent anticoagulant medication use\", \"Bleeding disorders\"]",
                standardPreparationForms: "Infusion, Decoction, Capsule, Powder",
                isApprovedForPrescription: true),

            HerbsCatalog.Create(
                scientificName: "Matricaria chamomilla",
                arabicCommonName: "بابونج",
                englishCommonName: "Chamomile",
                activeIngredients: "[\"Apigenin\", \"Chamazulene\", \"Bisabolol\"]",
                contraindicationsJson: "[\"Asteraceae family hypersensitivity\", \"Severe asthma history\"]",
                standardPreparationForms: "Infusion, Tincture, Topical Essential Oil",
                isApprovedForPrescription: true),

            HerbsCatalog.Create(
                scientificName: "Hypericum perforatum",
                arabicCommonName: "عشبة القديس يوحنا",
                englishCommonName: "St. John's Wort",
                activeIngredients: "[\"Hypericin\", \"Hyperforin\", \"Flavonoids\"]",
                contraindicationsJson: "[\"SSRIs and SNRIs (risk of Serotonin Syndrome)\", \"Oral contraceptives (reduced efficacy via CYP3A4 induction)\", \"Cyclosporine and Digoxin\"]",
                standardPreparationForms: "Standardized Extract, Capsule, Tincture",
                isApprovedForPrescription: true),

            HerbsCatalog.Create(
                scientificName: "Glycyrrhiza glabra",
                arabicCommonName: "عرقسوس",
                englishCommonName: "Licorice Root",
                activeIngredients: "[\"Glycyrrhizin\", \"Glabridin\", \"Liquiritin\"]",
                contraindicationsJson: "[\"Hypertension (pseudoaldosteronism risk)\", \"Hypokalemia\", \"Chronic kidney disease\", \"Heart failure\"]",
                standardPreparationForms: "Decoction, Fluid Extract, Chewable DGL",
                isApprovedForPrescription: true),

            HerbsCatalog.Create(
                scientificName: "Panax ginseng",
                arabicCommonName: "جنسنغ آسيوي",
                englishCommonName: "Panax Ginseng",
                activeIngredients: "[\"Ginsenosides Rg1, Rb1, Rd\", \"Panaxans\"]",
                contraindicationsJson: "[\"Uncontrolled hypertension\", \"Acute infection or fever\", \"Insomnia and anxiety\", \"MAOIs concurrent use\"]",
                standardPreparationForms: "Standardized Extract, Decoction, Capsule",
                isApprovedForPrescription: true)
        };

        foreach (var herb in herbsToSeed)
        {
            if (!await db.HerbsCatalog.AnyAsync(h => h.ScientificName == herb.ScientificName, ct))
            {
                db.HerbsCatalog.Add(herb);
            }
        }

        await db.SaveChangesAsync(ct);
    }
}
