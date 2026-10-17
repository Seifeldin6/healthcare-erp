namespace Erp.SharedKernel.Interfaces;

public interface IRegulatoryService
{
    Task<bool> ValidateLicenseAsync(string licenseNumber, string entityType, CancellationToken ct = default);
    Task<string> SubmitReportAsync(string reportType, object payload, CancellationToken ct = default);
}
