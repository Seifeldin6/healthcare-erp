using System.Security.Claims;
using System.Text.Json;
using Erp.Modules.Core.Data;
using Erp.Modules.Core.Entities;
using Erp.Modules.Core.Enums;
using Erp.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Erp.Modules.Core.Endpoints;

public static class PatientEndpoints
{
    public static IEndpointRouteBuilder MapPatientEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/core/patients");

        group.MapGet("/me/clinical-summary", GetMyClinicalSummary)
            .RequireAuthorization("PatientOnly")
            .WithName("GetMyClinicalSummary");

        group.MapPut("/me/clinical-summary", UpdateMyClinicalSummary)
            .RequireAuthorization("PatientOnly")
            .WithName("UpdateMyClinicalSummary");

        group.MapGet("/{patientId:guid}/clinical-summary", GetPatientClinicalSummary)
            .RequireAuthorization("ClinicalStaff")
            .WithName("GetPatientClinicalSummary");

        return app;
    }

    private static async Task<IResult> GetMyClinicalSummary(
        ClaimsPrincipal principal,
        CoreDbContext db,
        CancellationToken ct)
    {
        var userIdStr = principal.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
        if (!Guid.TryParse(userIdStr, out var userId))
            return Results.Unauthorized();

        var patient = await db.Patients.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userId, ct);
        if (patient is null)
            return Results.NotFound(new { message = "Patient profile not found." });

        var summary = await db.PatientClinicalSummaries
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.PatientId == patient.Id, ct);

        if (summary is null)
            return Results.NotFound(new { message = "Clinical summary not found." });

        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, ct);

        return Results.Ok(new
        {
            Patient = MapPatient(patient, user),
            ClinicalSummary = MapSummary(summary)
        });
    }

    private static async Task<IResult> UpdateMyClinicalSummary(
        UpdateClinicalSummaryRequest request,
        ClaimsPrincipal principal,
        CoreDbContext db,
        CancellationToken ct)
    {
        var userIdStr = principal.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
        if (!Guid.TryParse(userIdStr, out var userId))
            return Results.Unauthorized();

        var patient = await db.Patients.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userId, ct);
        if (patient is null)
            return Results.NotFound(new { message = "Patient profile not found." });

        // Validate and normalise each JSON field; null/empty defaults to "[]".
        if (!TryNormaliseJson(request.AllergiesJson, out var allergiesJson))
            return Results.BadRequest(new { message = "AllergiesJson is not valid JSON." });

        if (!TryNormaliseJson(request.ChronicConditionsJson, out var chronicConditionsJson))
            return Results.BadRequest(new { message = "ChronicConditionsJson is not valid JSON." });

        if (!TryNormaliseJson(request.ActiveMedicationsJson, out var activeMedicationsJson))
            return Results.BadRequest(new { message = "ActiveMedicationsJson is not valid JSON." });

        if (!TryNormaliseJson(request.MajorSurgeriesJson, out var majorSurgeriesJson))
            return Results.BadRequest(new { message = "MajorSurgeriesJson is not valid JSON." });

        var summary = await db.PatientClinicalSummaries
            .FirstOrDefaultAsync(s => s.PatientId == patient.Id, ct);

        if (summary is null)
        {
            // Upsert: create the record if it does not exist yet.
            summary = PatientClinicalSummary.Create(patient.Id);
            summary.Update(allergiesJson, chronicConditionsJson, activeMedicationsJson,
                request.ActiveHerbalPrescriptionsJson is null ? "[]" : NormaliseJsonOrDefault(request.ActiveHerbalPrescriptionsJson),
                majorSurgeriesJson);
            db.PatientClinicalSummaries.Add(summary);
        }
        else
        {
            summary.Update(
                allergiesJson,
                chronicConditionsJson,
                activeMedicationsJson,
                request.ActiveHerbalPrescriptionsJson is null
                    ? summary.ActiveHerbalPrescriptionsJson
                    : NormaliseJsonOrDefault(request.ActiveHerbalPrescriptionsJson),
                majorSurgeriesJson);
        }

        await db.SaveChangesAsync(ct);

        return Results.Ok(MapSummary(summary));
    }

    private static async Task<IResult> GetPatientClinicalSummary(
        Guid patientId,
        ClaimsPrincipal principal,
        CoreDbContext db,
        ICurrentUserService currentUser,
        CancellationToken ct)
    {
        var patient = await db.Patients.AsNoTracking().FirstOrDefaultAsync(p => p.Id == patientId, ct);
        if (patient is null)
            return Results.NotFound(new { message = "Patient not found." });

        var summary = await db.PatientClinicalSummaries
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.PatientId == patientId, ct);

        if (summary is null)
            return Results.NotFound(new { message = "Clinical summary not found." });

        var auditLog = AuditLog.Create(
            currentUser.UserId,
            currentUser.TenantId,
            moduleName: "Core",
            entityName: "PatientClinicalSummary",
            entityId: summary.Id.ToString(),
            AuditActionType.AccessClinicalSummary,
            oldValuesJson: null,
            newValuesJson: null);

        db.AuditLogs.Add(auditLog);
        await db.SaveChangesAsync(ct);

        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == patient.UserId, ct);

        return Results.Ok(new
        {
            Patient = MapPatient(patient, user),
            ClinicalSummary = MapSummary(summary)
        });
    }

    // Returns false when the string is non-null, non-empty, and not valid JSON.
    private static bool TryNormaliseJson(string? value, out string result)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = "[]";
            return true;
        }

        try
        {
            using var doc = JsonDocument.Parse(value);
            result = value;
            return true;
        }
        catch (JsonException)
        {
            result = string.Empty;
            return false;
        }
    }

    // For optional fields where we default to "[]" on invalid input rather than rejecting.
    private static string NormaliseJsonOrDefault(string value)
        => TryNormaliseJson(value, out var result) ? result : "[]";

    private static object MapPatient(Patient p, User? u) => new
    {
        p.Id,
        p.UserId,
        FullName = u?.FullName,
        Email = u?.Email,
        p.NationalId,
        p.DateOfBirth,
        Gender = p.Gender.ToString(),
        BloodType = p.BloodType.ToString(),
        p.WeightKg,
        p.HeightCm,
        p.EmergencyContactName,
        p.EmergencyContactPhone
    };

    private static object MapSummary(PatientClinicalSummary s) => new
    {
        s.Id,
        s.PatientId,
        s.AllergiesJson,
        s.ChronicConditionsJson,
        s.ActiveMedicationsJson,
        s.ActiveHerbalPrescriptionsJson,
        s.MajorSurgeriesJson,
        s.LastUpdatedAtUtc
    };

    public sealed record UpdateClinicalSummaryRequest(
        string AllergiesJson,
        string ChronicConditionsJson,
        string ActiveMedicationsJson,
        string MajorSurgeriesJson,
        string? ActiveHerbalPrescriptionsJson = null);
}
