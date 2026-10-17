using Erp.SharedKernel.Interfaces;

namespace Erp.Modules.Core.Gateways;

public sealed class MockPaymentGateway : IPaymentGateway
{
    public Task<string> CreatePaymentIntentAsync(decimal amount, string currency, string description, CancellationToken ct = default)
    {
        var mockTransactionId = $"mock_txn_{Guid.NewGuid():N}";
        return Task.FromResult(mockTransactionId);
    }

    public Task<bool> ConfirmPaymentAsync(string paymentIntentId, CancellationToken ct = default)
        => Task.FromResult(true);

    public Task<bool> RefundPaymentAsync(string paymentIntentId, decimal? amount = null, CancellationToken ct = default)
        => Task.FromResult(true);
}
