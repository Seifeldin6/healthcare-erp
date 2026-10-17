using Erp.Modules.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Modules.Core.Data.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs", "core");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedNever();

        builder.Property(a => a.UserId);

        builder.Property(a => a.TenantId);

        builder.Property(a => a.ModuleName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(a => a.EntityName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(a => a.EntityId)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(a => a.ActionType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(a => a.OldValuesJson)
            .HasColumnType("jsonb");

        builder.Property(a => a.NewValuesJson)
            .HasColumnType("jsonb");

        builder.Property(a => a.OccurredAtUtc)
            .IsRequired()
            .HasColumnType("timestamptz");

        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => a.TenantId);
        builder.HasIndex(a => a.OccurredAtUtc);
    }
}
