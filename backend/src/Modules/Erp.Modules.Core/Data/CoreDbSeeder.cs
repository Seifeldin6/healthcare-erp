using Erp.Modules.Core.Entities;
using Erp.Modules.Core.Enums;
using Erp.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Erp.Modules.Core.Data;

public static class CoreDbSeeder
{
    public static readonly Guid DefaultTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid DefaultPharmacistUserId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid DefaultPatientUserId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid DefaultPatientId = Guid.Parse("44444444-4444-4444-4444-444444444444");

    public static async Task SeedAsync(CoreDbContext db, IPasswordHasher passwordHasher, CancellationToken ct = default)
    {
        // 1. Seed Tenant
        if (!await db.Tenants.AnyAsync(t => t.Id == DefaultTenantId, ct))
        {
            var tenant = Tenant.Create(
                DefaultTenantId,
                name: "GreenLife Healthcare",
                tenantType: TenantType.Clinic,
                subscriptionPlan: SubscriptionPlan.Enterprise,
                enabledModulesJson: "[\"Core\", \"TelePharmacy\"]",
                subscriptionEndsAtUtc: DateTime.UtcNow.AddYears(1));

            db.Tenants.Add(tenant);
            await db.SaveChangesAsync(ct);
        }

        // 2. Seed Pharmacist User
        if (!await db.Users.AnyAsync(u => u.Email == "pharmacist@erp.local", ct))
        {
            var passwordHash = passwordHasher.Hash("Password123!");
            var pharmacistUser = User.Create(
                DefaultPharmacistUserId,
                DefaultTenantId,
                email: "pharmacist@erp.local",
                phoneNumber: "+201000000001",
                passwordHash: passwordHash,
                fullName: "Dr. Sarah Ahmed, PharmD",
                role: UserRole.Pharmacist);

            db.Users.Add(pharmacistUser);
            await db.SaveChangesAsync(ct);
        }

        // 3. Seed Patient User, Patient Profile, and Clinical Summary
        if (!await db.Users.AnyAsync(u => u.Email == "patient@erp.local", ct))
        {
            var passwordHash = passwordHasher.Hash("Password123!");
            var patientUser = User.Create(
                DefaultPatientUserId,
                tenantId: null,
                email: "patient@erp.local",
                phoneNumber: "+201000000002",
                passwordHash: passwordHash,
                fullName: "Ahmed Mahmoud",
                role: UserRole.Patient);

            var patient = Patient.Create(
                DefaultPatientId,
                patientUser.Id,
                nationalId: "29001011234567",
                dateOfBirth: new DateOnly(1990, 1, 1),
                gender: Gender.Male,
                bloodType: BloodType.APositive,
                weightKg: 75.5m,
                heightCm: 178.0m,
                emergencyContactName: "Mona Mahmoud",
                emergencyContactPhone: "+201000000003");

            var clinicalSummary = PatientClinicalSummary.Create(patient.Id);
            clinicalSummary.Update(
                allergiesJson: "[\"Penicillin\", \"Sulfa Drugs\", \"Peanuts\"]",
                chronicConditionsJson: "[\"Mild Hypertension\", \"Seasonal Rhinitis\"]",
                activeMedicationsJson: "[\"Amlodipine 5mg daily\", \"Cetirizine 10mg PRN\"]",
                activeHerbalPrescriptionsJson: "[]",
                majorSurgeriesJson: "[\"Appendectomy (2018)\"]");

            db.Users.Add(patientUser);
            db.Patients.Add(patient);
            db.PatientClinicalSummaries.Add(clinicalSummary);
            await db.SaveChangesAsync(ct);
        }
    }
}
