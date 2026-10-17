using Erp.Modules.TelePharmacy.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Modules.TelePharmacy.Data.Configurations;

public class ConsultationConfiguration : IEntityTypeConfiguration<Consultation>
{
    public void Configure(EntityTypeBuilder<Consultation> builder)
    {
        builder.ToTable("consultations", "telepharmacy");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.PatientId)
            .IsRequired();

        builder.HasIndex(c => c.PatientId);

        builder.Property(c => c.PharmacistProfileId)
            .IsRequired();

        builder.HasIndex(c => c.PharmacistProfileId);

        builder.Property(c => c.TenantId);

        builder.HasIndex(c => c.TenantId);

        builder.Property(c => c.AgreedFeeAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(c => c.ChiefComplaintAndSymptoms)
            .IsRequired();

        builder.Property(c => c.ClinicalSummaryConsentGranted)
            .IsRequired();

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.RejectionReason);

        builder.Property(c => c.IsDeleted)
            .IsRequired();

        builder.Property(c => c.RequestedAtUtc)
            .IsRequired()
            .HasColumnType("timestamptz");

        builder.Property(c => c.ExpiresAtUtc)
            .IsRequired()
            .HasColumnType("timestamptz");

        builder.Property(c => c.AcceptedAtUtc)
            .HasColumnType("timestamptz");

        builder.Property(c => c.CompletedAtUtc)
            .HasColumnType("timestamptz");

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
