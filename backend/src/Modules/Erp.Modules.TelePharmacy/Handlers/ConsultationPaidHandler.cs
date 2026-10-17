using Erp.Modules.TelePharmacy.Data;
using Erp.Modules.TelePharmacy.Entities;
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
            return;

        consultation.ActivateChat();

        var chatRoom = ChatRoom.Create(consultation.Id);
        _db.ChatRooms.Add(chatRoom);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
