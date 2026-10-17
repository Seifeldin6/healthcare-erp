using Erp.Modules.Core.Enums;
using Erp.SharedKernel.Entities;

namespace Erp.Modules.Core.Entities;

public class Patient : BaseEntity
{
    public Guid UserId { get; private set; }
    public string NationalId { get; private set; } = string.Empty;
    public DateOnly DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public BloodType BloodType { get; private set; }
    public decimal WeightKg { get; private set; }
    public decimal HeightCm { get; private set; }
    public string? EmergencyContactName { get; private set; }
    public string? EmergencyContactPhone { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    private Patient() { }

    public static Patient Create(
        Guid userId,
        string nationalId,
        DateOnly dateOfBirth,
        Gender gender,
        BloodType bloodType,
        decimal weightKg,
        decimal heightCm,
        string? emergencyContactName,
        string? emergencyContactPhone)
    {
        return new Patient
        {
            UserId = userId,
            NationalId = nationalId,
            DateOfBirth = dateOfBirth,
            Gender = gender,
            BloodType = bloodType,
            WeightKg = weightKg,
            HeightCm = heightCm,
            EmergencyContactName = emergencyContactName,
            EmergencyContactPhone = emergencyContactPhone
        };
    }
}
