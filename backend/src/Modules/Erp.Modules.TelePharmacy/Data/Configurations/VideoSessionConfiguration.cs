using Erp.Modules.TelePharmacy.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Modules.TelePharmacy.Data.Configurations;

public class VideoSessionConfiguration : IEntityTypeConfiguration<VideoSession>
{
    public void Configure(EntityTypeBuilder<VideoSession> builder)
    {
        builder.ToTable("video_sessions", "telepharmacy");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .ValueGeneratedNever();

        builder.Property(v => v.ConsultationId)
            .IsRequired();

        builder.HasIndex(v => v.ConsultationId);

        builder.Property(v => v.TriggeredByPharmacistUserId)
            .IsRequired();

        builder.HasIndex(v => v.TriggeredByPharmacistUserId);

        builder.Property(v => v.ProviderType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(v => v.ExternalRoomName)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(v => v.SessionStatus)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(v => v.StartedAtUtc)
            .HasColumnType("timestamptz");

        builder.Property(v => v.EndedAtUtc)
            .HasColumnType("timestamptz");

        builder.Property(v => v.DurationSeconds);
    }
}
