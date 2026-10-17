using Erp.Modules.Core.Enums;
using Erp.SharedKernel.Entities;

namespace Erp.Modules.Core.Entities;

public class Tenant : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public TenantType TenantType { get; private set; }
    public SubscriptionPlan SubscriptionPlan { get; private set; }
    public string EnabledModulesJson { get; private set; } = "[]";
    public bool IsActive { get; private set; } = true;
    public DateTime? SubscriptionEndsAtUtc { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    private Tenant() { }

    public static Tenant Create(
        string name,
        TenantType tenantType,
        SubscriptionPlan subscriptionPlan,
        string enabledModulesJson,
        DateTime? subscriptionEndsAtUtc)
    {
        return new Tenant
        {
            Name = name,
            TenantType = tenantType,
            SubscriptionPlan = subscriptionPlan,
            EnabledModulesJson = enabledModulesJson,
            SubscriptionEndsAtUtc = subscriptionEndsAtUtc
        };
    }
}
