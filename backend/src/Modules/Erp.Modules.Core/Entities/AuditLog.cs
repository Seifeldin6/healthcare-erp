using Erp.Modules.Core.Enums;
using Erp.SharedKernel.Entities;

namespace Erp.Modules.Core.Entities;

public class AuditLog : BaseEntity
{
    public Guid? UserId { get; private set; }
    public Guid? TenantId { get; private set; }
    public string ModuleName { get; private set; } = string.Empty;
    public string EntityName { get; private set; } = string.Empty;
    public string EntityId { get; private set; } = string.Empty;
    public AuditActionType ActionType { get; private set; }
    public string? OldValuesJson { get; private set; }
    public string? NewValuesJson { get; private set; }
    public DateTime OccurredAtUtc { get; private set; } = DateTime.UtcNow;

    private AuditLog() { }

    public static AuditLog Create(
        Guid? userId,
        Guid? tenantId,
        string moduleName,
        string entityName,
        string entityId,
        AuditActionType actionType,
        string? oldValuesJson,
        string? newValuesJson)
    {
        return new AuditLog
        {
            UserId = userId,
            TenantId = tenantId,
            ModuleName = moduleName,
            EntityName = entityName,
            EntityId = entityId,
            ActionType = actionType,
            OldValuesJson = oldValuesJson,
            NewValuesJson = newValuesJson
        };
    }
}
