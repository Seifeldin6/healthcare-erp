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
            .Produces<ErrorResponse>(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .Produces<ErrorResponse>(StatusCodes.Status409Conflict);

        group.MapPost("/mock-pay", Checkout)
            .RequireAuthorization("PatientOnly")
            .WithName("MockPay")
            .Produces<PaymentCheckoutResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status401Unauthorized)
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
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Results.Json(ErrorResponse.Unauthorized("Your session has expired. Please sign in again.", code: "SESSION_EXPIRED", traceId: httpContext.TraceIdentifier), statusCode: StatusCodes.Status401Unauthorized);

        var patient = await db.Patients.AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == currentUser.UserId.Value, ct);
        if (patient is null)
            return Results.NotFound(ErrorResponse.NotFound("Patient profile could not be found.", code: "PATIENT_NOT_FOUND", traceId: httpContext.TraceIdentifier));

        if (request.Amount <= 0)
            return Results.BadRequest(ErrorResponse.BadRequest("Payment amount must be greater than zero.", code: "INVALID_AMOUNT", traceId: httpContext.TraceIdentifier));

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
            await publisher.Publish(integrationEvent, ct);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("was not found", StringComparison.OrdinalIgnoreCase))
            {
                return Results.NotFound(ErrorResponse.NotFound(
                    "The requested consultation could not be found.",
                    code: "CONSULTATION_NOT_FOUND",
                    traceId: httpContext.TraceIdentifier));
            }

            if (ex.Message.Contains("does not belong", StringComparison.OrdinalIgnoreCase))
            {
                return Results.Json(ErrorResponse.Forbidden(
                    "You do not have permission to perform this action.",
                    code: "FORBIDDEN",
                    traceId: httpContext.TraceIdentifier),
                    statusCode: StatusCodes.Status403Forbidden);
            }

            return Results.Conflict(ErrorResponse.Conflict(
                ex.Message,
                code: "INVALID_STATUS_TRANSITION",
                traceId: httpContext.TraceIdentifier));
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
