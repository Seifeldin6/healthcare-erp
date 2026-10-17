using Erp.Modules.Core.Enums;
using Erp.SharedKernel.Entities;

namespace Erp.Modules.Core.Entities;

public class User : BaseEntity
{
    public Guid? TenantId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    private User() { }

    public static User Create(
        Guid? tenantId,
        string email,
        string phoneNumber,
        string passwordHash,
        string fullName,
        UserRole role)
    {
        return new User
        {
            TenantId = tenantId,
            Email = email,
            PhoneNumber = phoneNumber,
            PasswordHash = passwordHash,
            FullName = fullName,
            Role = role
        };
    }
}
