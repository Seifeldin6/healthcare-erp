using Erp.SharedKernel.Entities;

namespace Erp.Modules.TelePharmacy.Entities;

public class ChatRoom : BaseEntity
{
    public Guid ConsultationId { get; private set; }
    public bool IsReadOnly { get; private set; }
    public DateTime OpenedAtUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? ClosedAtUtc { get; private set; }

    private ChatRoom() { }

    public static ChatRoom Create(Guid consultationId)
    {
        return new ChatRoom { ConsultationId = consultationId };
    }

    public void Close()
    {
        IsReadOnly = true;
        ClosedAtUtc = DateTime.UtcNow;
    }
}
