using MediatR;

namespace Erp.SharedKernel.Events;

public sealed record ConsultationPaidIntegrationEvent(
    Guid ConsultationId,
    Guid PatientId,
    Guid PaymentId,
    decimal Amount,
    DateTimeOffset PaidAtUtc) : INotification;
