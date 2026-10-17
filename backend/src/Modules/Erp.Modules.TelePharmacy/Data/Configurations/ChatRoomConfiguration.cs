using Erp.Modules.TelePharmacy.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Modules.TelePharmacy.Data.Configurations;

public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.ToTable("chat_rooms", "telepharmacy");

        builder.HasKey(cr => cr.Id);

        builder.Property(cr => cr.Id)
            .ValueGeneratedNever();

        builder.Property(cr => cr.ConsultationId)
            .IsRequired();

        builder.HasIndex(cr => cr.ConsultationId)
            .IsUnique();

        builder.Property(cr => cr.IsReadOnly)
            .IsRequired();

        builder.Property(cr => cr.OpenedAtUtc)
            .IsRequired()
            .HasColumnType("timestamptz");

        builder.Property(cr => cr.ClosedAtUtc)
            .HasColumnType("timestamptz");
    }
}
