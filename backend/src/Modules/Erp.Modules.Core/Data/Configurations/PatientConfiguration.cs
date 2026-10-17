using Erp.Modules.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Modules.Core.Data.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("patients", "core");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.HasIndex(p => p.UserId)
            .IsUnique();

        builder.Property(p => p.NationalId)
            .IsRequired()
            .HasMaxLength(64);

        builder.HasIndex(p => p.NationalId)
            .IsUnique();

        builder.Property(p => p.DateOfBirth)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(p => p.Gender)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.BloodType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.WeightKg)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.HeightCm)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.EmergencyContactName)
            .HasMaxLength(256);

        builder.Property(p => p.EmergencyContactPhone)
            .HasMaxLength(32);

        builder.Property(p => p.IsDeleted)
            .IsRequired();

        builder.Property(p => p.CreatedAtUtc)
            .IsRequired()
            .HasColumnType("timestamptz");

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
