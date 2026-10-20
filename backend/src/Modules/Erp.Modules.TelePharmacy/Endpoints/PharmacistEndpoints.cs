using Erp.Modules.TelePharmacy.Data;
using Erp.Modules.TelePharmacy.Entities;
using Erp.Modules.TelePharmacy.Enums;
using Erp.SharedKernel.Interfaces;
using Erp.SharedKernel.Pagination;
using Erp.SharedKernel.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Erp.Modules.TelePharmacy.Endpoints;

public static class PharmacistEndpoints
{
    public static IEndpointRouteBuilder MapPharmacistEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/telepharmacy/pharmacists");

        group.MapGet("/", GetApprovedAvailablePharmacists)
            .RequireAuthorization()
            .WithName("GetApprovedAvailablePharmacists")
            .Produces<PagedResult<PharmacistResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/me", GetMyPharmacistProfile)
            .RequireAuthorization("PharmacistOnly")
            .WithName("GetMyPharmacistProfile")
            .Produces<PharmacistResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        group.MapPost("/me/profile", UpsertMyPharmacistProfile)
            .RequireAuthorization("PharmacistOnly")
            .WithName("CreateMyPharmacistProfile")
            .Produces<PharmacistResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResponse>(StatusCodes.Status409Conflict);

        group.MapPut("/me/profile", UpsertMyPharmacistProfile)
            .RequireAuthorization("PharmacistOnly")
            .WithName("UpdateMyPharmacistProfile")
            .Produces<PharmacistResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResponse>(StatusCodes.Status409Conflict);

        group.MapPut("/me/availability", ToggleMyAvailability)
            .RequireAuthorization("PharmacistOnly")
            .WithName("ToggleMyAvailability")
            .Produces<PharmacistAvailabilityResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        group.MapPost("/{id:guid}/verify", VerifyPharmacist)
            .RequireAuthorization()
            .WithName("VerifyPharmacist")
            .Produces<PharmacistResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

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
            return Results.NotFound(new ErrorResponse("Pharmacist profile not found."));

        return Results.Ok(MapProfile(profile));
    }

    private static async Task<IResult> UpsertMyPharmacistProfile(
        UpsertPharmacistProfileRequest request,
        ICurrentUserService currentUser,
        TelePharmacyDbContext db,
        IWebHostEnvironment environment,
        CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Results.Unauthorized();

        var profile = await db.PharmacistProfiles
            .FirstOrDefaultAsync(p => p.UserId == currentUser.UserId.Value && !p.IsDeleted, ct);

        var licenseNumber = !string.IsNullOrWhiteSpace(request.LicenseNumber)
            ? request.LicenseNumber
            : request.SyndicateLicenseNumber;

        var issuingDoc = !string.IsNullOrWhiteSpace(request.IssuingAuthority)
            ? request.IssuingAuthority
            : request.LicenseDocumentUrl;

        if (profile is null)
        {
            if (string.IsNullOrWhiteSpace(licenseNumber))
                return Results.BadRequest(new ErrorResponse("License number is required."));

            var licenseExists = await db.PharmacistProfiles
                .AsNoTracking()
                .AnyAsync(p => p.SyndicateLicenseNumber == licenseNumber && !p.IsDeleted, ct);

            if (licenseExists)
                return Results.Conflict(new ErrorResponse("License number is already registered."));

            profile = PharmacistProfile.Create(
                currentUser.UserId.Value,
                currentUser.TenantId,
                licenseNumber,
                issuingDoc,
                request.Specialization,
                request.Bio,
                request.ConsultationFee);

            if (environment.IsDevelopment())
            {
                profile.Approve();
                profile.SetAvailability(true);
            }

            db.PharmacistProfiles.Add(profile);
        }
        else
        {
            profile.UpdateDetails(
                request.Bio,
                request.ConsultationFee,
                request.Specialization,
                issuingDoc);
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
            return Results.NotFound(new ErrorResponse("Pharmacist profile not found."));

        var targetAvailability = request?.IsAvailable ?? !profile.IsAvailableForRequests;

        if (targetAvailability && profile.VerificationStatus != VerificationStatus.Approved)
        {
            return Results.BadRequest(new ErrorResponse("Only approved pharmacists can set availability to true."));
        }

        profile.SetAvailability(targetAvailability);
        await db.SaveChangesAsync(ct);

        return Results.Ok(new PharmacistAvailabilityResponse(
            profile.Id,
            profile.IsAvailableForRequests,
            profile.VerificationStatus.ToString()));
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
            return Results.NotFound(new ErrorResponse("Pharmacist profile not found."));

        var approve = request?.Approve ?? true;

        if (approve)
        {
            profile.Approve();
            profile.SetAvailability(true);
        }
        else
        {
            profile.Reject();
            profile.SetAvailability(false);
        }

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

    public sealed record PharmacistAvailabilityResponse(
        Guid PharmacistProfileId,
        bool IsAvailableForRequests,
        string VerificationStatus);

    public sealed record UpdateAvailabilityRequest(bool? IsAvailable = null);

    public sealed record UpsertPharmacistProfileRequest(
        string? LicenseNumber = null,
        string? SyndicateLicenseNumber = null,
        string? IssuingAuthority = null,
        string? LicenseDocumentUrl = null,
        PharmacistSpecialization Specialization = PharmacistSpecialization.ClinicalPharmacy,
        string? Bio = null,
        decimal ConsultationFee = 0m);

    public sealed record VerifyPharmacistRequest(bool Approve = true, string? Reason = null);
}
