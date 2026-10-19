using System.Text.Json;
using Erp.Modules.Core.Data;
using Erp.Modules.Core.Entities;
using Erp.SharedKernel.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Erp.Modules.Core.Handlers;

public sealed class HerbalPrescriptionIssuedHandler : INotificationHandler<HerbalPrescriptionIssuedIntegrationEvent>
{
    private readonly CoreDbContext _db;

    public HerbalPrescriptionIssuedHandler(CoreDbContext db)
    {
        _db = db;
    }

    public async Task Handle(HerbalPrescriptionIssuedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var summary = await _db.PatientClinicalSummaries
            .FirstOrDefaultAsync(s => s.PatientId == notification.PatientId && !s.IsDeleted, cancellationToken);

        if (summary is null)
        {
            summary = PatientClinicalSummary.Create(notification.PatientId);
            _db.PatientClinicalSummaries.Add(summary);
        }

        var prescriptionsList = new List<object>();

        if (!string.IsNullOrWhiteSpace(summary.ActiveHerbalPrescriptionsJson))
        {
            try
            {
                using var existingDoc = JsonDocument.Parse(summary.ActiveHerbalPrescriptionsJson);
                if (existingDoc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var element in existingDoc.RootElement.EnumerateArray())
                    {
                        var deserialized = JsonSerializer.Deserialize<object>(element.GetRawText());
                        if (deserialized is not null)
                            prescriptionsList.Add(deserialized);
                    }
                }
            }
            catch (JsonException)
            {
                // In case of corrupted existing JSON, reset to fresh list
            }
        }

        object herbsPayload;
        try
        {
            herbsPayload = JsonSerializer.Deserialize<object>(notification.PrescribedHerbsJson)
                ?? notification.PrescribedHerbsJson;
        }
        catch (JsonException)
        {
            herbsPayload = notification.PrescribedHerbsJson;
        }

        prescriptionsList.Add(new
        {
            notification.PrescriptionId,
            notification.ConsultationId,
            notification.PharmacistProfileId,
            notification.PrescriptionNumber,
            Herbs = herbsPayload,
            notification.IssuedAtUtc
        });

        var updatedHerbalJson = JsonSerializer.Serialize(prescriptionsList);

        summary.Update(
            summary.AllergiesJson,
            summary.ChronicConditionsJson,
            summary.ActiveMedicationsJson,
            updatedHerbalJson,
            summary.MajorSurgeriesJson);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
