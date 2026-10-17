using Erp.SharedKernel.Interfaces;

namespace Erp.SharedKernel.Entities;

public abstract class BaseEntity : IAuditable, ISoftDeletable
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; protected set; } = DateTimeOffset.UtcNow;
    public string? CreatedBy { get; protected set; }
    public string? UpdatedBy { get; protected set; }

    public bool IsDeleted { get; protected set; }
    public DateTimeOffset? DeletedAt { get; protected set; }

    public void SoftDelete(string? deletedBy = null)
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.UtcNow;
        UpdatedBy = deletedBy;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
