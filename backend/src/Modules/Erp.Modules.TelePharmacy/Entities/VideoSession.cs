using Erp.Modules.TelePharmacy.Enums;
using Erp.SharedKernel.Entities;

namespace Erp.Modules.TelePharmacy.Entities;

public class VideoSession : BaseEntity
{
    public Guid ConsultationId { get; private set; }
    public Guid TriggeredByPharmacistUserId { get; private set; }
    public VideoProviderType ProviderType { get; private set; }
    public string ExternalRoomName { get; private set; } = string.Empty;
    public VideoSessionStatus SessionStatus { get; private set; } = VideoSessionStatus.Ringing;
    public DateTime? StartedAtUtc { get; private set; }
    public DateTime? EndedAtUtc { get; private set; }
    public int? DurationSeconds { get; private set; }

    private VideoSession() { }

    public static VideoSession Create(
        Guid consultationId,
        Guid triggeredByPharmacistUserId,
        VideoProviderType providerType,
        string externalRoomName)
    {
        return new VideoSession
        {
            ConsultationId = consultationId,
            TriggeredByPharmacistUserId = triggeredByPharmacistUserId,
            ProviderType = providerType,
            ExternalRoomName = externalRoomName
        };
    }

    public void Start()
    {
        SessionStatus = VideoSessionStatus.Active;
        StartedAtUtc = DateTime.UtcNow;
    }

    public void End()
    {
        SessionStatus = VideoSessionStatus.Ended;
        EndedAtUtc = DateTime.UtcNow;
        if (StartedAtUtc.HasValue)
            DurationSeconds = (int)(EndedAtUtc.Value - StartedAtUtc.Value).TotalSeconds;
    }

    public void MarkAsMissed() => SessionStatus = VideoSessionStatus.Missed;
}
