namespace Erp.SharedKernel.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    Guid? TenantId { get; }
    Guid? PatientId { get; }
    string? Role { get; }
    bool IsAuthenticated { get; }
}
