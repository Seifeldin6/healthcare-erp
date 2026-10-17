using Erp.Modules.TelePharmacy.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Modules.TelePharmacy.Data.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("chat_messages", "telepharmacy");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever();

        builder.Property(m => m.ChatRoomId)
            .IsRequired();

        builder.HasIndex(m => m.ChatRoomId);

        builder.Property(m => m.SenderUserId)
            .IsRequired();

        builder.HasIndex(m => m.SenderUserId);

        builder.Property(m => m.SenderRole)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(m => m.MessageType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(m => m.MessageContent);

        builder.Property(m => m.AttachmentUrl)
            .HasMaxLength(2048);

        builder.Property(m => m.IsRead)
            .IsRequired();

        builder.Property(m => m.SentAtUtc)
            .IsRequired()
            .HasColumnType("timestamptz");
    }
}
