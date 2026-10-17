using Erp.Modules.TelePharmacy.Enums;
using Erp.SharedKernel.Entities;

namespace Erp.Modules.TelePharmacy.Entities;

public class ChatMessage : BaseEntity
{
    public Guid ChatRoomId { get; private set; }
    public Guid SenderUserId { get; private set; }
    public ChatSenderRole SenderRole { get; private set; }
    public ChatMessageType MessageType { get; private set; }
    public string? MessageContent { get; private set; }
    public string? AttachmentUrl { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime SentAtUtc { get; private set; } = DateTime.UtcNow;

    private ChatMessage() { }

    public static ChatMessage Create(
        Guid chatRoomId,
        Guid senderUserId,
        ChatSenderRole senderRole,
        ChatMessageType messageType,
        string? messageContent,
        string? attachmentUrl)
    {
        return new ChatMessage
        {
            ChatRoomId = chatRoomId,
            SenderUserId = senderUserId,
            SenderRole = senderRole,
            MessageType = messageType,
            MessageContent = messageContent,
            AttachmentUrl = attachmentUrl
        };
    }

    public void MarkAsRead() => IsRead = true;
}
