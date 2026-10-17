using Erp.Modules.TelePharmacy.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Modules.TelePharmacy.Data.Configurations;

public class HerbalPrescriptionConfiguration : IEntityTypeConfiguration<HerbalPrescription>
{
    public void Configure(EntityTypeBuilder<HerbalPrescription> builder)
    {
        builder.ToTable("herbal_prescriptions", "telepharmacy");

        builder.HasKey(hp => hp.Id);

        builder.Property(hp => hp.Id)
            .ValueGeneratedNever();

        builder.Property(hp => hp.ConsultationId)
            .IsRequired();

        builder.HasIndex(hp => hp.ConsultationId)
            .IsUnique();

        builder.Property(hp => hp.PatientId)
            .IsRequired();

        builder.HasIndex(hp => hp.PatientId);

        builder.Property(hp => hp.PharmacistProfileId)
            .IsRequired();

        builder.HasIndex(hp => hp.PharmacistProfileId);

        builder.Property(hp => hp.PrescriptionNumber)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasIndex(hp => hp.PrescriptionNumber)
            .IsUnique();

        builder.Property(hp => hp.ClinicalDiagnosisSummary);

        builder.Property(hp => hp.PrescribedHerbsJson)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(hp => hp.DrugHerbInteractionWarnings);

        builder.Property(hp => hp.DietaryAndLifestyleInstructions);

        builder.Property(hp => hp.DurationDays)
            .IsRequired();

        builder.Property(hp => hp.PdfDocumentUrl)
            .HasMaxLength(2048);

        builder.Property(hp => hp.IsDeleted)
            .IsRequired();

        builder.Property(hp => hp.IssuedAtUtc)
            .IsRequired()
            .HasColumnType("timestamptz");

        builder.HasQueryFilter(hp => !hp.IsDeleted);
    }
}
