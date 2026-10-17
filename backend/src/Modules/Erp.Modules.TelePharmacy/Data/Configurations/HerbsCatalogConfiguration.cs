using Erp.Modules.TelePharmacy.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Modules.TelePharmacy.Data.Configurations;

public class HerbsCatalogConfiguration : IEntityTypeConfiguration<HerbsCatalog>
{
    public void Configure(EntityTypeBuilder<HerbsCatalog> builder)
    {
        builder.ToTable("herbs_catalog", "telepharmacy");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
            .ValueGeneratedNever();

        builder.Property(h => h.ScientificName)
            .IsRequired()
            .HasMaxLength(512);

        builder.HasIndex(h => h.ScientificName)
            .IsUnique();

        builder.Property(h => h.ArabicCommonName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(h => h.EnglishCommonName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(h => h.ActiveIngredients)
            .HasMaxLength(2048);

        builder.Property(h => h.ContraindicationsJson)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(h => h.StandardPreparationForms)
            .HasMaxLength(512);

        builder.Property(h => h.IsApprovedForPrescription)
            .IsRequired();

        builder.Property(h => h.IsDeleted)
            .IsRequired();

        builder.HasQueryFilter(h => !h.IsDeleted);
    }
}
