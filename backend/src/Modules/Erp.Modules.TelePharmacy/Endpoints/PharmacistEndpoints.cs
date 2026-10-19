using Erp.Modules.TelePharmacy.Data;
using Erp.Modules.TelePharmacy.Entities;
using Erp.Modules.TelePharmacy.Enums;
using Erp.SharedKernel.Interfaces;
using Erp.SharedKernel.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Erp.Modules.TelePharmacy.Endpoints;

public static class PharmacistEndpoints
{
    public static IEndpointRouteBuilder MapPharmacistEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/telepharmacy/pharmacists");

        group.MapGet("/", GetApprovedAvailablePharmacists)
            .RequireAuthorization()
            .WithName("GetApprovedAvailablePharmacists");

        group.MapGet("/me", GetMyPharmacistProfile)
            .RequireAuthorization("PharmacistOnly")
            .WithName("GetMyPharmacistProfile");

        group.MapPost("/me/profile", UpsertMyPharmacistProfile)
            .RequireAuthorization("PharmacistOnly")
            .WithName("UpsertMyPharmacistProfile");

        group.MapPut("/me/availability", ToggleMyAvailability)
            .RequireAuthorization("PharmacistOnly")
            .WithName("ToggleMyAvailability");

        group.MapPost("/{id:guid}/verify", VerifyPharmacist)
            .RequireAuthorization()
            .WithName("VerifyPharmacist");

        return app;
    }

    private static async Task<IResult> GetApprovedAvailablePharmacists(
        [AsParameters] PagedRequest pagedRequest,
        PharmacistSpecialization? specialization,
        TelePharmacyDbContext db,
        CancellationToken ct)
    {
        var query = db.PharmacistProfiles
            .AsNoTracking()
            .Where(p => !p.IsDeleted &&
                        p.VerificationStatus == VerificationStatus.Approved &&
                        p.IsAvailableForRequests);

        if (specialization.HasValue)
            query = query.Where(p => p.Specialization == specialization.Value);

        query = query.OrderByDescending(p => p.VerifiedAtUtc);

        var result = await query
            .Select(p => new PharmacistResponse(
                p.Id,
                p.UserId,
                p.TenantId,
                p.SyndicateLicenseNumber,
                p.LicenseDocumentUrl,
                p.VerificationStatus.ToString(),
                p.Specialization.ToString(),
                p.Bio,
                p.ConsultationFee,
                p.IsAvailableForRequests,
                p.VerifiedAtUtc))
            .ToPagedResultAsync(pagedRequest, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetMyPharmacistProfile(
        ICurrentUserService currentUser,
        TelePharmacyDbContext db,
        CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Results.Unauthorized();

        var profile = await db.PharmacistProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == currentUser.UserId.Value && !p.IsDeleted, ct);

        if (profile is null)
            return Results.NotFound(new { message = "Pharmacist profile not found." });

        return Results.Ok(MapProfile(profile));
    }

    private static async Task<IResult> UpsertMyPharmacistProfile(
        UpsertPharmacistProfileRequest request,
        ICurrentUserService currentUser,
        TelePharmacyDbContext db,
        CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Results.Unauthorized();

        var profile = await db.PharmacistProfiles
            .FirstOrDefaultAsync(p => p.UserId == currentUser.UserId.Value && !p.IsDeleted, ct);

        if (profile is null)
        {
            if (string.IsNullOrWhiteSpace(request.SyndicateLicenseNumber))
                return Results.BadRequest(new { message = "Syndicate license number is required." });

            var licenseExists = await db.PharmacistProfiles
                .AsNoTracking()
                .AnyAsync(p => p.SyndicateLicenseNumber == request.SyndicateLicenseNumber && !p.IsDeleted, ct);

            if (licenseExists)
                return Results.Conflict(new { message = "Syndicate license number is already registered." });

            profile = PharmacistProfile.Create(
                currentUser.UserId.Value,
                currentUser.TenantId,
                request.SyndicateLicenseNumber,
                request.LicenseDocumentUrl,
                request.Specialization,
                request.Bio,
                request.ConsultationFee);

            db.PharmacistProfiles.Add(profile);
        }
        else
        {
            profile.UpdateDetails(
                request.Bio,
                request.ConsultationFee,
                request.Specialization,
                request.LicenseDocumentUrl);
        }

        await db.SaveChangesAsync(ct);
        return Results.Ok(MapProfile(profile));
    }

    private static async Task<IResult> ToggleMyAvailability(
        UpdateAvailabilityRequest? request,
        ICurrentUserService currentUser,
        TelePharmacyDbContext db,
        CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Results.Unauthorized();

        var profile = await db.PharmacistProfiles
            .FirstOrDefaultAsync(p => p.UserId == currentUser.UserId.Value && !p.IsDeleted, ct);

        if (profile is null)
            return Results.NotFound(new { message = "Pharmacist profile not found." });

        var targetAvailability = request?.IsAvailable ?? !profile.IsAvailableForRequests;

        if (targetAvailability && profile.VerificationStatus != VerificationStatus.Approved)
        {
            return Results.BadRequest(new
            {
                message = "Only approved pharmacists can set availability to true."
            });
        }

        profile.SetAvailability(targetAvailability);
        await db.SaveChangesAsync(ct);

        return Results.Ok(new
        {
            PharmacistProfileId = profile.Id,
            profile.IsAvailableForRequests,
            VerificationStatus = profile.VerificationStatus.ToString()
        });
    }

    private static async Task<IResult> VerifyPharmacist(
        Guid id,
        VerifyPharmacistRequest? request,
        TelePharmacyDbContext db,
        CancellationToken ct)
    {
        var profile = await db.PharmacistProfiles
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, ct);

        if (profile is null)
            return Results.NotFound(new { message = "Pharmacist profile not found." });

        var approve = request?.Approve ?? true;

        if (approve)
            profile.Approve();
        else
            profile.Reject();

        await db.SaveChangesAsync(ct);

        return Results.Ok(MapProfile(profile));
    }

    private static PharmacistResponse MapProfile(PharmacistProfile p) => new(
        p.Id,
        p.UserId,
        p.TenantId,
        p.SyndicateLicenseNumber,
        p.LicenseDocumentUrl,
        p.VerificationStatus.ToString(),
        p.Specialization.ToString(),
        p.Bio,
        p.ConsultationFee,
        p.IsAvailableForRequests,
        p.VerifiedAtUtc);

    public sealed record PharmacistResponse(
        Guid Id,
        Guid UserId,
        Guid? TenantId,
        string SyndicateLicenseNumber,
        string? LicenseDocumentUrl,
        string VerificationStatus,
        string Specialization,
        string? Bio,
        decimal ConsultationFee,
        bool IsAvailableForRequests,
        DateTime? VerifiedAtUtc);

    public sealed record UpdateAvailabilityRequest(bool? IsAvailable = null);

    public sealed record UpsertPharmacistProfileRequest(
        string SyndicateLicenseNumber,
        string? LicenseDocumentUrl,
        PharmacistSpecialization Specialization,
        string? Bio,
        decimal ConsultationFee);

    public sealed record VerifyPharmacistRequest(bool Approve = true, string? Reason = null);
}
