using MediatR;

namespace Erp.SharedKernel.Events;

public sealed record HerbalPrescriptionIssuedIntegrationEvent(
    Guid PrescriptionId,
    Guid ConsultationId,
    Guid PatientId,
    Guid PharmacistProfileId,
    string PrescriptionNumber,
    string PrescribedHerbsJson,
    DateTimeOffset IssuedAtUtc) : INotification;
