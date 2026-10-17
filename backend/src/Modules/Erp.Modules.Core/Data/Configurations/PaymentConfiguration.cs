using Erp.Modules.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Modules.Core.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments", "core");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.PatientId)
            .IsRequired();

        builder.HasIndex(p => p.PatientId);

        builder.Property(p => p.TenantId);

        builder.Property(p => p.ReferenceEntityId)
            .IsRequired();

        builder.Property(p => p.ReferenceModule)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.Currency)
            .IsRequired()
            .HasMaxLength(8);

        builder.Property(p => p.GatewayProvider)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(p => p.GatewayTransactionId)
            .HasMaxLength(256);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.CreatedAtUtc)
            .IsRequired()
            .HasColumnType("timestamptz");

        builder.Property(p => p.PaidAtUtc)
            .HasColumnType("timestamptz");
    }
}
