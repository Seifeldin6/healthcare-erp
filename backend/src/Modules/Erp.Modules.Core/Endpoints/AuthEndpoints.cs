using Erp.Modules.Core.Data;
using Erp.Modules.Core.Entities;
using Erp.Modules.Core.Enums;
using Erp.Modules.Core.Services;
using Erp.SharedKernel.Interfaces;
using Erp.SharedKernel.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Erp.Modules.Core.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/core/auth");

        group.MapPost("/register-patient", RegisterPatient)
            .AllowAnonymous()
            .WithName("RegisterPatient")
            .Produces<RegisterPatientResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status409Conflict);

        group.MapPost("/register-pharmacist", RegisterPharmacist)
            .AllowAnonymous()
            .WithName("RegisterPharmacist")
            .Produces<RegisterPharmacistResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status409Conflict);

        group.MapPost("/login", Login)
            .AllowAnonymous()
            .WithName("Login")
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResponse>(StatusCodes.Status403Forbidden);

        return app;
    }

    private static async Task<IResult> RegisterPatient(
        RegisterPatientRequest request,
        CoreDbContext db,
        IPasswordHasher passwordHasher,
        JwtTokenGenerator tokenGenerator,
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (await db.Users.AsNoTracking().AnyAsync(u => u.Email == request.Email, ct))
            return Results.Conflict(ErrorResponse.Conflict("This email address is already registered.", code: "EMAIL_ALREADY_EXISTS", traceId: httpContext.TraceIdentifier));

        if (await db.Patients.AsNoTracking().AnyAsync(p => p.NationalId == request.NationalId, ct))
            return Results.Conflict(ErrorResponse.Conflict("This national ID is already registered.", code: "NATIONAL_ID_ALREADY_EXISTS", traceId: httpContext.TraceIdentifier));

        var passwordHash = passwordHasher.Hash(request.Password);
        var user = User.Create(null, request.Email, request.PhoneNumber, passwordHash, request.FullName, UserRole.Patient);

        var patient = Patient.Create(
            user.Id,
            request.NationalId,
            request.DateOfBirth,
            request.Gender,
            request.BloodType,
            request.WeightKg,
            request.HeightCm,
            request.EmergencyContactName,
            request.EmergencyContactPhone);

        var clinicalSummary = PatientClinicalSummary.Create(patient.Id);

        db.Users.Add(user);
        db.Patients.Add(patient);
        db.PatientClinicalSummaries.Add(clinicalSummary);
        await db.SaveChangesAsync(ct);

        var token = tokenGenerator.GenerateToken(user, patient.Id);

        return Results.Ok(new RegisterPatientResponse(
            token,
            user.Id,
            patient.Id,
            user.Role.ToString()));
    }

    private static async Task<IResult> RegisterPharmacist(
        RegisterPharmacistRequest request,
        CoreDbContext db,
        IPasswordHasher passwordHasher,
        JwtTokenGenerator tokenGenerator,
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (await db.Users.AsNoTracking().AnyAsync(u => u.Email == request.Email, ct))
            return Results.Conflict(ErrorResponse.Conflict("This email address is already registered.", code: "EMAIL_ALREADY_EXISTS", traceId: httpContext.TraceIdentifier));

        var passwordHash = passwordHasher.Hash(request.Password);
        var user = User.Create(request.TenantId, request.Email, request.PhoneNumber, passwordHash, request.FullName, UserRole.Pharmacist);

        db.Users.Add(user);
        await db.SaveChangesAsync(ct);

        var token = tokenGenerator.GenerateToken(user);

        return Results.Ok(new RegisterPharmacistResponse(
            token,
            user.Id,
            user.Role.ToString(),
            user.TenantId));
    }

    private static async Task<IResult> Login(
        LoginRequest request,
        CoreDbContext db,
        IPasswordHasher passwordHasher,
        JwtTokenGenerator tokenGenerator,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(
            u => u.Email == request.EmailOrPhone || u.PhoneNumber == request.EmailOrPhone, ct);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            return Results.Json(
                ErrorResponse.Unauthorized("Invalid email or password.", code: "INVALID_CREDENTIALS", traceId: httpContext.TraceIdentifier),
                statusCode: StatusCodes.Status401Unauthorized);

        if (!user.IsActive)
            return Results.Json(
                ErrorResponse.Forbidden("Your account has been deactivated. Please contact support.", code: "ACCOUNT_DEACTIVATED", traceId: httpContext.TraceIdentifier),
                statusCode: StatusCodes.Status403Forbidden);

        Guid? patientId = null;
        if (user.Role == UserRole.Patient)
        {
            var patient = await db.Patients.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == user.Id, ct);
            patientId = patient?.Id;
        }

        var token = tokenGenerator.GenerateToken(user, patientId);

        return Results.Ok(new LoginResponse(
            token,
            user.Id,
            user.Role.ToString(),
            user.TenantId,
            patientId));
    }

    public sealed record RegisterPatientRequest(
        string Email,
        string PhoneNumber,
        string Password,
        string FullName,
        string NationalId,
        DateOnly DateOfBirth,
        Gender Gender,
        BloodType BloodType,
        decimal WeightKg,
        decimal HeightCm,
        string? EmergencyContactName = null,
        string? EmergencyContactPhone = null);

    public sealed record RegisterPatientResponse(
        string Token,
        Guid UserId,
        Guid PatientId,
        string Role);

    public sealed record RegisterPharmacistRequest(
        string Email,
        string PhoneNumber,
        string Password,
        string FullName,
        Guid? TenantId = null);

    public sealed record RegisterPharmacistResponse(
        string Token,
        Guid UserId,
        string Role,
        Guid? TenantId);

    public sealed record LoginRequest(
        string EmailOrPhone,
        string Password);

    public sealed record LoginResponse(
        string Token,
        Guid UserId,
        string Role,
        Guid? TenantId,
        Guid? PatientId);
}
