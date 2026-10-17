namespace Erp.SharedKernel.Interfaces;

public interface IAuditable
{
    DateTimeOffset CreatedAt { get; }
    DateTimeOffset UpdatedAt { get; }
    string? CreatedBy { get; }
    string? UpdatedBy { get; }
}
