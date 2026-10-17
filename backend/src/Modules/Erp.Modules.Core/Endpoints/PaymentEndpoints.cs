using Erp.Modules.Core.Data;
using Erp.Modules.Core.Entities;
using Erp.Modules.Core.Enums;
using Erp.SharedKernel.Events;
using Erp.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Erp.Modules.Core.Endpoints;

public static class PaymentEndpoints
{
    public static IEndpointRouteBuilder MapPaymentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/core/payments");

        group.MapPost("/mock-pay", MockPay)
            .RequireAuthorization("PatientOnly")
            .WithName("MockPay");

        return app;
    }

    private static async Task<IResult> MockPay(
        MockPayRequest request,
        ICurrentUserService currentUser,
        CoreDbContext db,
        IPaymentGateway paymentGateway,
        IPublisher publisher,
        CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Results.Unauthorized();

        var patient = await db.Patients.FirstOrDefaultAsync(p => p.UserId == currentUser.UserId.Value, ct);
        if (patient is null)
            return Results.NotFound(new { message = "Patient profile not found." });

        var transactionId = await paymentGateway.CreatePaymentIntentAsync(
            request.Amount, request.Currency, $"Consultation {request.ConsultationId}", ct);
        await paymentGateway.ConfirmPaymentAsync(transactionId, ct);

        var payment = Payment.Create(
            patient.Id,
            currentUser.TenantId,
            request.ConsultationId,
            referenceModule: "TelePharmacy",
            request.Amount,
            request.Currency,
            gatewayProvider: "MockGateway",
            gatewayTransactionId: transactionId);

        payment.MarkAsPaid(transactionId);

        db.Payments.Add(payment);
        await db.SaveChangesAsync(ct);

        var integrationEvent = new ConsultationPaidIntegrationEvent(
            request.ConsultationId,
            patient.Id,
            payment.Id,
            request.Amount,
            payment.PaidAtUtc.HasValue
                ? new DateTimeOffset(payment.PaidAtUtc.Value, TimeSpan.Zero)
                : DateTimeOffset.UtcNow);

        await publisher.Publish(integrationEvent, ct);

        return Results.Ok(new
        {
            PaymentId = payment.Id,
            Status = PaymentStatus.Paid.ToString(),
            payment.GatewayTransactionId
        });
    }

    public sealed record MockPayRequest(
        Guid ConsultationId,
        decimal Amount,
        string Currency = "EGP");
}
