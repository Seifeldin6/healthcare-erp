using Erp.Modules.Core.Data;
using Erp.Modules.Core.Entities;
using Erp.Modules.Core.Enums;
using Erp.SharedKernel.Events;
using Erp.SharedKernel.Interfaces;
using Erp.SharedKernel.Responses;
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

        group.MapPost("/checkout", Checkout)
            .RequireAuthorization("PatientOnly")
            .WithName("Checkout")
            .Produces<PaymentCheckoutResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .Produces<ErrorResponse>(StatusCodes.Status409Conflict);

        group.MapPost("/mock-pay", Checkout)
            .RequireAuthorization("PatientOnly")
            .WithName("MockPay")
            .Produces<PaymentCheckoutResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .Produces<ErrorResponse>(StatusCodes.Status409Conflict);

        return app;
    }

    private static async Task<IResult> Checkout(
        PaymentCheckoutRequest request,
        ICurrentUserService currentUser,
        CoreDbContext db,
        IPaymentGateway paymentGateway,
        IPublisher publisher,
        CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Results.Unauthorized();

        var patient = await db.Patients.AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == currentUser.UserId.Value, ct);
        if (patient is null)
            return Results.NotFound(new ErrorResponse("Patient profile not found."));

        if (request.Amount <= 0)
            return Results.BadRequest(new ErrorResponse("Payment amount must be greater than zero."));

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

        // Build the integration event before saving so the handler can validate
        // that the consultation exists and is in the correct state. If the handler
        // throws, SaveChangesAsync is never called and no phantom payment is persisted.
        var integrationEvent = new ConsultationPaidIntegrationEvent(
            request.ConsultationId,
            patient.Id,
            payment.Id,
            request.Amount,
            payment.PaidAtUtc.HasValue
                ? new DateTimeOffset(payment.PaidAtUtc.Value, TimeSpan.Zero)
                : DateTimeOffset.UtcNow);

        try
        {
            // Validate via the TelePharmacy handler BEFORE committing the Core payment row.
            await publisher.Publish(integrationEvent, ct);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(new ErrorResponse(ex.Message));
        }

        db.Payments.Add(payment);
        await db.SaveChangesAsync(ct);

        return Results.Ok(new PaymentCheckoutResponse(
            payment.Id,
            PaymentStatus.Paid.ToString(),
            payment.GatewayTransactionId));
    }

    public sealed record PaymentCheckoutRequest(
        Guid ConsultationId,
        decimal Amount,
        string Currency = "EGP");

    public sealed record PaymentCheckoutResponse(
        Guid PaymentId,
        string Status,
        string? GatewayTransactionId);

    public sealed record MockPayRequest(
        Guid ConsultationId,
        decimal Amount,
        string Currency = "EGP");
}
