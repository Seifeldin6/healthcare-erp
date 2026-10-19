using Erp.Modules.TelePharmacy.Data;
using Erp.Modules.TelePharmacy.Entities;
using Erp.Modules.TelePharmacy.Enums;
using Erp.SharedKernel.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Erp.Modules.TelePharmacy.Handlers;

public sealed class ConsultationPaidHandler : INotificationHandler<ConsultationPaidIntegrationEvent>
{
    private readonly TelePharmacyDbContext _db;

    public ConsultationPaidHandler(TelePharmacyDbContext db)
    {
        _db = db;
    }

    public async Task Handle(ConsultationPaidIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var consultation = await _db.Consultations
            .FirstOrDefaultAsync(c => c.Id == notification.ConsultationId, cancellationToken);

        if (consultation is null)
            throw new InvalidOperationException(
                $"Consultation {notification.ConsultationId} was not found in TelePharmacy.");

        if (consultation.PatientId != notification.PatientId)
            throw new InvalidOperationException(
                $"Consultation {notification.ConsultationId} does not belong to patient {notification.PatientId}.");

        if (consultation.Status != ConsultationStatus.AcceptedAwaitingPayment)
            throw new InvalidOperationException(
                $"Consultation {notification.ConsultationId} is not in the AcceptedAwaitingPayment state " +
                $"(current: {consultation.Status}).");

        consultation.ActivateChat();

        // Idempotent: only insert a ChatRoom if one does not already exist.
        var existingRoom = await _db.ChatRooms
            .FirstOrDefaultAsync(r => r.ConsultationId == notification.ConsultationId, cancellationToken);

        if (existingRoom is null)
        {
            var chatRoom = ChatRoom.Create(consultation.Id);
            _db.ChatRooms.Add(chatRoom);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
