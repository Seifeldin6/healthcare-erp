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
            .FirstOrDefaultAsync(r => r.Id == chatRoomId && !r.IsDeleted, Context.ConnectionAborted);

        if (chatRoom is null)
            throw new HubException("Chat room not found.");

        await ValidateChatRoomAccessAsync(chatRoom.ConsultationId);

        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString(), Context.ConnectionAborted);

        await Clients.Caller.SendAsync("JoinedRoom", new
        {
            ChatRoomId = chatRoomId,
            chatRoom.IsReadOnly
        }, Context.ConnectionAborted);
    }

    public async Task LeaveRoom(Guid chatRoomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId.ToString(), Context.ConnectionAborted);
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
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == chatRoomId && !r.IsDeleted, Context.ConnectionAborted);

        if (chatRoom is null)
            throw new HubException("Chat room not found.");

        if (chatRoom.IsReadOnly)
            throw new HubException("Chat room is read-only because the consultation is completed.");

        await ValidateChatRoomAccessAsync(chatRoom.ConsultationId);

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
        await _db.SaveChangesAsync(Context.ConnectionAborted);

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

        await Clients.Group(chatRoomId.ToString()).SendAsync("ReceiveMessage", broadcastPayload, Context.ConnectionAborted);
    }

    private async Task ValidateChatRoomAccessAsync(Guid consultationId)
    {
        var consultation = await _db.Consultations
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == consultationId && !c.IsDeleted, Context.ConnectionAborted);

        if (consultation is null)
            throw new HubException("Consultation not found.");

        var role = Context.User?.FindFirstValue(ClaimTypes.Role);

        if (string.Equals(role, "SuperAdmin", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(role, "Doctor", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (string.Equals(role, "Patient", StringComparison.OrdinalIgnoreCase))
        {
            var patientIdClaim = Context.User?.FindFirstValue("patient_id");
            if (Guid.TryParse(patientIdClaim, out var patientId) && patientId == consultation.PatientId)
                return;

            throw new HubException("Access denied.");
        }

        if (string.Equals(role, "Pharmacist", StringComparison.OrdinalIgnoreCase))
        {
            var userIdStr = Context.User?.FindFirstValue("sub")
                ?? Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Guid.TryParse(userIdStr, out var userId))
            {
                var profile = await _db.PharmacistProfiles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDeleted, Context.ConnectionAborted);

                if (profile is not null && profile.Id == consultation.PharmacistProfileId)
                    return;
            }

            throw new HubException("Access denied.");
        }

        throw new HubException("Access denied.");
    }
}
