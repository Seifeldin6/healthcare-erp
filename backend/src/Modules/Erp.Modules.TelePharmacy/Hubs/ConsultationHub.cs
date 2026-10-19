using System.Security.Claims;
using Erp.Modules.TelePharmacy.Data;
using Erp.Modules.TelePharmacy.Entities;
using Erp.Modules.TelePharmacy.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Erp.Modules.TelePharmacy.Hubs;

[Authorize]
public class ConsultationHub : Hub
{
    private readonly TelePharmacyDbContext _db;

    public ConsultationHub(TelePharmacyDbContext db)
    {
        _db = db;
    }

    public async Task JoinRoom(Guid chatRoomId)
    {
        var chatRoom = await _db.ChatRooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == chatRoomId && !r.IsDeleted);

        if (chatRoom is null)
            throw new HubException("Chat room not found.");

        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString());

        await Clients.Caller.SendAsync("JoinedRoom", new
        {
            ChatRoomId = chatRoomId,
            chatRoom.IsReadOnly
        });
    }

    public async Task LeaveRoom(Guid chatRoomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId.ToString());
    }

    public async Task SendMessage(
        Guid chatRoomId,
        string messageContent,
        string messageType,
        string? attachmentUrl)
    {
        var userIdStr = Context.User?.FindFirstValue("sub")
            ?? Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? Context.UserIdentifier;

        if (!Guid.TryParse(userIdStr, out var userId))
            throw new HubException("Unauthorized: missing user identifier.");

        var chatRoom = await _db.ChatRooms
            .FirstOrDefaultAsync(r => r.Id == chatRoomId && !r.IsDeleted);

        if (chatRoom is null)
            throw new HubException("Chat room not found.");

        if (chatRoom.IsReadOnly)
            throw new HubException("Chat room is read-only because the consultation is completed.");

        var roleStr = Context.User?.FindFirstValue(ClaimTypes.Role);
        var senderRole = Enum.TryParse<ChatSenderRole>(roleStr, true, out var role)
            ? role
            : ChatSenderRole.System;

        var parsedMessageType = Enum.TryParse<ChatMessageType>(messageType, true, out var parsedType)
            ? parsedType
            : ChatMessageType.Text;

        var message = ChatMessage.Create(
            chatRoomId,
            userId,
            senderRole,
            parsedMessageType,
            messageContent,
            attachmentUrl);

        _db.ChatMessages.Add(message);
        await _db.SaveChangesAsync();

        var broadcastPayload = new
        {
            message.Id,
            message.ChatRoomId,
            message.SenderUserId,
            SenderRole = message.SenderRole.ToString(),
            MessageType = message.MessageType.ToString(),
            message.MessageContent,
            message.AttachmentUrl,
            message.SentAtUtc,
            message.IsRead
        };

        await Clients.Group(chatRoomId.ToString()).SendAsync("ReceiveMessage", broadcastPayload);
    }
}
