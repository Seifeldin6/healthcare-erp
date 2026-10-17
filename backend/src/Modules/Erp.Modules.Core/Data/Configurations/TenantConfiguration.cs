using Erp.Modules.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Modules.Core.Data.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("tenants", "core");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(t => t.TenantType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.SubscriptionPlan)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.EnabledModulesJson)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(t => t.IsActive)
            .IsRequired();

        builder.Property(t => t.IsDeleted)
            .IsRequired();

        builder.Property(t => t.SubscriptionEndsAtUtc)
            .HasColumnType("timestamptz");

        builder.Property(t => t.CreatedAtUtc)
            .IsRequired()
            .HasColumnType("timestamptz");

        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}
