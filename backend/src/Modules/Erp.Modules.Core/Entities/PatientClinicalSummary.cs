using Erp.SharedKernel.Entities;

namespace Erp.Modules.Core.Entities;

public class PatientClinicalSummary : BaseEntity
{
    public Guid PatientId { get; private set; }
    public string AllergiesJson { get; private set; } = "[]";
    public string ChronicConditionsJson { get; private set; } = "[]";
    public string ActiveMedicationsJson { get; private set; } = "[]";
    public string ActiveHerbalPrescriptionsJson { get; private set; } = "[]";
    public string MajorSurgeriesJson { get; private set; } = "[]";
    public DateTime LastUpdatedAtUtc { get; private set; } = DateTime.UtcNow;

    private PatientClinicalSummary() { }

    public static PatientClinicalSummary Create(Guid patientId)
    {
        return new PatientClinicalSummary { PatientId = patientId };
    }

    public void Update(
        string allergiesJson,
        string chronicConditionsJson,
        string activeMedicationsJson,
        string activeHerbalPrescriptionsJson,
        string majorSurgeriesJson)
    {
        AllergiesJson = allergiesJson;
        ChronicConditionsJson = chronicConditionsJson;
        ActiveMedicationsJson = activeMedicationsJson;
        ActiveHerbalPrescriptionsJson = activeHerbalPrescriptionsJson;
        MajorSurgeriesJson = majorSurgeriesJson;
        LastUpdatedAtUtc = DateTime.UtcNow;
    }
}
