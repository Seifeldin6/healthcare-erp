using Erp.Modules.TelePharmacy.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Modules.TelePharmacy.Data.Configurations;

public class PharmacistProfileConfiguration : IEntityTypeConfiguration<PharmacistProfile>
{
    public void Configure(EntityTypeBuilder<PharmacistProfile> builder)
    {
        builder.ToTable("pharmacist_profiles", "telepharmacy");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.HasIndex(p => p.UserId)
            .IsUnique();

        builder.Property(p => p.TenantId);

        builder.HasIndex(p => p.TenantId);

        builder.Property(p => p.SyndicateLicenseNumber)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasIndex(p => p.SyndicateLicenseNumber)
            .IsUnique();

        builder.Property(p => p.LicenseDocumentUrl)
            .HasMaxLength(2048);

        builder.Property(p => p.VerificationStatus)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.Specialization)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.Bio)
            .HasMaxLength(2048);

        builder.Property(p => p.ConsultationFee)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.IsAvailableForRequests)
            .IsRequired();

        builder.Property(p => p.IsDeleted)
            .IsRequired();

        builder.Property(p => p.VerifiedAtUtc)
            .HasColumnType("timestamptz");

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
