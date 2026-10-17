using Erp.Modules.Core.Enums;
using Erp.SharedKernel.Entities;

namespace Erp.Modules.Core.Entities;

public class Payment : BaseEntity
{
    public Guid PatientId { get; private set; }
    public Guid? TenantId { get; private set; }
    public Guid ReferenceEntityId { get; private set; }
    public string ReferenceModule { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "EGP";
    public string GatewayProvider { get; private set; } = string.Empty;
    public string? GatewayTransactionId { get; private set; }
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? PaidAtUtc { get; private set; }

    private Payment() { }

    public static Payment Create(
        Guid patientId,
        Guid? tenantId,
        Guid referenceEntityId,
        string referenceModule,
        decimal amount,
        string currency,
        string gatewayProvider,
        string? gatewayTransactionId)
    {
        return new Payment
        {
            PatientId = patientId,
            TenantId = tenantId,
            ReferenceEntityId = referenceEntityId,
            ReferenceModule = referenceModule,
            Amount = amount,
            Currency = currency,
            GatewayProvider = gatewayProvider,
            GatewayTransactionId = gatewayTransactionId
        };
    }

    public void MarkAsPaid(string gatewayTransactionId)
    {
        Status = PaymentStatus.Paid;
        GatewayTransactionId = gatewayTransactionId;
        PaidAtUtc = DateTime.UtcNow;
    }

    public void MarkAsFailed() => Status = PaymentStatus.Failed;

    public void MarkAsRefunded() => Status = PaymentStatus.Refunded;
}
