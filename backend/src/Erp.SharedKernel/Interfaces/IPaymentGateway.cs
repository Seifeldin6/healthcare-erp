namespace Erp.SharedKernel.Interfaces;

public interface IPaymentGateway
{
    Task<string> CreatePaymentIntentAsync(decimal amount, string currency, string description, CancellationToken ct = default);
    Task<bool> ConfirmPaymentAsync(string paymentIntentId, CancellationToken ct = default);
    Task<bool> RefundPaymentAsync(string paymentIntentId, decimal? amount = null, CancellationToken ct = default);
}
