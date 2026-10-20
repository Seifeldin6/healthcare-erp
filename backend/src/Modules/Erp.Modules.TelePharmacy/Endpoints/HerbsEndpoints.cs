using Erp.Modules.TelePharmacy.Data;
using Erp.Modules.TelePharmacy.Entities;
using Erp.SharedKernel.Pagination;
using Erp.SharedKernel.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Erp.Modules.TelePharmacy.Endpoints;

public static class HerbsEndpoints
{
    public static IEndpointRouteBuilder MapHerbsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/telepharmacy/herbs");

        group.MapGet("/", GetHerbs)
            .RequireAuthorization()
            .WithName("GetHerbs")
            .Produces<PagedResult<HerbResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/{id:guid}", GetHerbById)
            .RequireAuthorization()
            .WithName("GetHerbById")
            .Produces<HerbResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateHerb)
            .RequireAuthorization("ClinicalStaff")
            .WithName("CreateHerb")
            .Produces<HerbResponse>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        return app;
    }

    private static async Task<IResult> GetHerbs(
        [AsParameters] PagedRequest pagedRequest,
        string? search,
        bool? isApprovedForPrescription,
        TelePharmacyDbContext db,
        CancellationToken ct)
    {
        var query = db.HerbsCatalog.AsNoTracking().Where(h => !h.IsDeleted);

        if (isApprovedForPrescription.HasValue)
            query = query.Where(h => h.IsApprovedForPrescription == isApprovedForPrescription.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(h =>
                EF.Functions.ILike(h.ScientificName, $"%{term}%") ||
                EF.Functions.ILike(h.ArabicCommonName, $"%{term}%") ||
                EF.Functions.ILike(h.EnglishCommonName, $"%{term}%"));
        }

        query = query.OrderBy(h => h.EnglishCommonName);

        var result = await query
            .Select(h => new HerbResponse(
                h.Id,
                h.ScientificName,
                h.ArabicCommonName,
                h.EnglishCommonName,
                h.ActiveIngredients,
                h.ContraindicationsJson,
                h.StandardPreparationForms,
                h.IsApprovedForPrescription))
            .ToPagedResultAsync(pagedRequest, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetHerbById(
        Guid id,
        TelePharmacyDbContext db,
        CancellationToken ct)
    {
        var herb = await db.HerbsCatalog
            .AsNoTracking()
            .Where(h => h.Id == id && !h.IsDeleted)
            .Select(h => new HerbResponse(
                h.Id,
                h.ScientificName,
                h.ArabicCommonName,
                h.EnglishCommonName,
                h.ActiveIngredients,
                h.ContraindicationsJson,
                h.StandardPreparationForms,
                h.IsApprovedForPrescription))
            .FirstOrDefaultAsync(ct);

        if (herb is null)
            return Results.NotFound(new ErrorResponse("Herb not found."));

        return Results.Ok(herb);
    }

    private static async Task<IResult> CreateHerb(
        CreateHerbRequest request,
        TelePharmacyDbContext db,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.ScientificName) ||
            string.IsNullOrWhiteSpace(request.EnglishCommonName) ||
            string.IsNullOrWhiteSpace(request.ArabicCommonName))
        {
            return Results.BadRequest(new ErrorResponse("Scientific name, English common name, and Arabic common name are required."));
        }

        var contraindications = string.IsNullOrWhiteSpace(request.ContraindicationsJson) ? "[]" : request.ContraindicationsJson;

        var herb = HerbsCatalog.Create(
            request.ScientificName,
            request.ArabicCommonName,
            request.EnglishCommonName,
            request.ActiveIngredients,
            contraindications,
            request.StandardPreparationForms,
            request.IsApprovedForPrescription);

        db.HerbsCatalog.Add(herb);
        await db.SaveChangesAsync(ct);

        var response = new HerbResponse(
            herb.Id,
            herb.ScientificName,
            herb.ArabicCommonName,
            herb.EnglishCommonName,
            herb.ActiveIngredients,
            herb.ContraindicationsJson,
            herb.StandardPreparationForms,
            herb.IsApprovedForPrescription);

        return Results.Created($"/api/telepharmacy/herbs/{herb.Id}", response);
    }

    public sealed record HerbResponse(
        Guid Id,
        string ScientificName,
        string ArabicCommonName,
        string EnglishCommonName,
        string? ActiveIngredients,
        string ContraindicationsJson,
        string? StandardPreparationForms,
        bool IsApprovedForPrescription);

    public sealed record CreateHerbRequest(
        string ScientificName,
        string ArabicCommonName,
        string EnglishCommonName,
        string? ActiveIngredients,
        string? ContraindicationsJson,
        string? StandardPreparationForms,
        bool IsApprovedForPrescription = true);
}
