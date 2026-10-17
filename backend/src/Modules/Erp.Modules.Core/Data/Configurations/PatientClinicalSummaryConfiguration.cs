using Erp.Modules.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Modules.Core.Data.Configurations;

public class PatientClinicalSummaryConfiguration : IEntityTypeConfiguration<PatientClinicalSummary>
{
    public void Configure(EntityTypeBuilder<PatientClinicalSummary> builder)
    {
        builder.ToTable("patient_clinical_summaries", "core");

        builder.HasKey(pcs => pcs.Id);

        builder.Property(pcs => pcs.Id)
            .ValueGeneratedNever();

        builder.Property(pcs => pcs.PatientId)
            .IsRequired();

        builder.HasIndex(pcs => pcs.PatientId)
            .IsUnique();

        builder.Property(pcs => pcs.AllergiesJson)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(pcs => pcs.ChronicConditionsJson)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(pcs => pcs.ActiveMedicationsJson)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(pcs => pcs.ActiveHerbalPrescriptionsJson)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(pcs => pcs.MajorSurgeriesJson)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(pcs => pcs.LastUpdatedAtUtc)
            .IsRequired()
            .HasColumnType("timestamptz");
    }
}
