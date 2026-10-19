using System.Security.Claims;
using System.Text.Json;
using Erp.Modules.TelePharmacy.Data;
using Erp.Modules.TelePharmacy.Entities;
using Erp.Modules.TelePharmacy.Enums;
using Erp.Modules.TelePharmacy.Hubs;
using Erp.SharedKernel.Events;
using Erp.SharedKernel.Interfaces;
using Erp.SharedKernel.Pagination;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Erp.Modules.TelePharmacy.Endpoints;

public static class ConsultationEndpoints
{
    public static IEndpointRouteBuilder MapConsultationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/telepharmacy/consultations");

        group.MapPost("/", CreateConsultation)
            .RequireAuthorization("PatientOnly")
            .WithName("CreateConsultation");

        group.MapPost("/{id:guid}/accept", AcceptConsultation)
            .RequireAuthorization("PharmacistOnly")
            .WithName("AcceptConsultation");

        group.MapPost("/{id:guid}/reject", RejectConsultation)
            .RequireAuthorization("PharmacistOnly")
            .WithName("RejectConsultation");

        group.MapGet("/", GetMyConsultations)
            .RequireAuthorization(policy => policy.RequireRole("Patient", "Pharmacist", "SuperAdmin", "Doctor"))
            .WithName("GetMyConsultations");

        group.MapGet("/{id:guid}", GetConsultationById)
            .RequireAuthorization()
            .WithName("GetConsultationById");

        group.MapPost("/{id:guid}/start-video", StartVideo)
            .RequireAuthorization("PharmacistOnly")
            .WithName("StartVideo");

        group.MapPost("/{id:guid}/end-video", EndVideo)
            .RequireAuthorization("PharmacistOnly")
            .WithName("EndVideo");

        group.MapPost("/{id:guid}/herbal-prescription", IssueHerbalPrescription)
            .RequireAuthorization("PharmacistOnly")
            .WithName("IssueHerbalPrescription");

        group.MapGet("/{id:guid}/messages", GetConsultationMessages)
            .RequireAuthorization()
            .WithName("GetConsultationMessages");

        return app;
    }

    private static async Task<IResult> CreateConsultation(
        CreateConsultationRequest request,
        ICurrentUserService currentUser,
        TelePharmacyDbContext db,
        CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Results.Unauthorized();

        var patientId = currentUser.PatientId ?? request.PatientId;
        if (!patientId.HasValue || patientId.Value == Guid.Empty)
            return Results.BadRequest(new { message = "Patient identifier could not be determined." });

        if (string.IsNullOrWhiteSpace(request.ChiefComplaintAndSymptoms))
            return Results.BadRequest(new { message = "Chief complaint and symptoms are required." });

        var pharmacist = await db.PharmacistProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.PharmacistProfileId && !p.IsDeleted, ct);

        if (pharmacist is null)
            return Results.NotFound(new { message = "Pharmacist profile not found." });

        if (pharmacist.VerificationStatus != VerificationStatus.Approved || !pharmacist.IsAvailableForRequests)
            return Results.BadRequest(new { message = "Pharmacist is not currently available for consultation requests." });

        var expiresAtUtc = DateTime.UtcNow.AddHours(request.DurationHours ?? 24);

        var consultation = Consultation.Create(
            patientId.Value,
            pharmacist.Id,
            pharmacist.TenantId,
            pharmacist.ConsultationFee,
            request.ChiefComplaintAndSymptoms,
            request.ClinicalSummaryConsentGranted,
            expiresAtUtc);

        db.Consultations.Add(consultation);
        await db.SaveChangesAsync(ct);

        return Results.Created($"/api/telepharmacy/consultations/{consultation.Id}", MapConsultation(consultation));
    }

    private static async Task<IResult> AcceptConsultation(
        Guid id,
        AcceptConsultationRequest? request,
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

        var consultation = await db.Consultations
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);

        if (consultation is null)
            return Results.NotFound(new { message = "Consultation not found." });

        if (consultation.PharmacistProfileId != profile.Id)
            return Results.Forbid();

        if (consultation.Status != ConsultationStatus.Pending)
        {
            return Results.Conflict(new
            {
                message = $"Cannot accept consultation in status '{consultation.Status}'. Must be 'Pending'."
            });
        }

        if (consultation.ExpiresAtUtc <= DateTime.UtcNow)
        {
            consultation.Expire();
            await db.SaveChangesAsync(ct);
            return Results.Conflict(new { message = "Consultation request has expired." });
        }

        var agreedFee = request?.Fee ?? profile.ConsultationFee;
        consultation.Accept(agreedFee);

        await db.SaveChangesAsync(ct);

        return Results.Ok(MapConsultation(consultation));
    }

    private static async Task<IResult> RejectConsultation(
        Guid id,
        RejectConsultationRequest request,
        ICurrentUserService currentUser,
        TelePharmacyDbContext db,
        CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Results.Unauthorized();

        if (string.IsNullOrWhiteSpace(request.RejectionReason))
            return Results.BadRequest(new { message = "Rejection reason is required." });

        var profile = await db.PharmacistProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == currentUser.UserId.Value && !p.IsDeleted, ct);

        if (profile is null)
            return Results.NotFound(new { message = "Pharmacist profile not found." });

        var consultation = await db.Consultations
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);

        if (consultation is null)
            return Results.NotFound(new { message = "Consultation not found." });

        if (consultation.PharmacistProfileId != profile.Id)
            return Results.Forbid();

        if (consultation.Status != ConsultationStatus.Pending)
        {
            return Results.Conflict(new
            {
                message = $"Cannot reject consultation in status '{consultation.Status}'. Must be 'Pending'."
            });
        }

        consultation.Reject(request.RejectionReason);
        await db.SaveChangesAsync(ct);

        return Results.Ok(MapConsultation(consultation));
    }

    private static async Task<IResult> GetMyConsultations(
        [AsParameters] PagedRequest pagedRequest,
        ConsultationStatus? status,
        ICurrentUserService currentUser,
        TelePharmacyDbContext db,
        CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Results.Unauthorized();

        var query = db.Consultations.AsNoTracking().Where(c => !c.IsDeleted);

        if (string.Equals(currentUser.Role, "Patient", StringComparison.OrdinalIgnoreCase))
        {
            var patientId = currentUser.PatientId;
            if (!patientId.HasValue)
                return Results.BadRequest(new { message = "Patient claim missing from token." });

            query = query.Where(c => c.PatientId == patientId.Value);
        }
        else if (string.Equals(currentUser.Role, "Pharmacist", StringComparison.OrdinalIgnoreCase))
        {
            var profile = await db.PharmacistProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == currentUser.UserId.Value && !p.IsDeleted, ct);

            if (profile is null)
                return Results.NotFound(new { message = "Pharmacist profile not found." });

            query = query.Where(c => c.PharmacistProfileId == profile.Id);
        }

        if (status.HasValue)
            query = query.Where(c => c.Status == status.Value);

        query = query.OrderByDescending(c => c.RequestedAtUtc);

        var result = await query
            .Select(c => new ConsultationResponse(
                c.Id,
                c.PatientId,
                c.PharmacistProfileId,
                c.TenantId,
                c.AgreedFeeAmount,
                c.ChiefComplaintAndSymptoms,
                c.ClinicalSummaryConsentGranted,
                c.Status.ToString(),
                c.RejectionReason,
                c.RequestedAtUtc,
                c.ExpiresAtUtc,
                c.AcceptedAtUtc,
                c.CompletedAtUtc))
            .ToPagedResultAsync(pagedRequest, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetConsultationById(
        Guid id,
        ICurrentUserService currentUser,
        TelePharmacyDbContext db,
        CancellationToken ct)
    {
        var consultation = await db.Consultations
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);

        if (consultation is null)
            return Results.NotFound(new { message = "Consultation not found." });

        var chatRoom = await db.ChatRooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.ConsultationId == consultation.Id && !r.IsDeleted, ct);

        var prescription = await db.HerbalPrescriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ConsultationId == consultation.Id && !p.IsDeleted, ct);

        var latestVideoSession = await db.VideoSessions
            .AsNoTracking()
            .Where(v => v.ConsultationId == consultation.Id && !v.IsDeleted)
            .OrderByDescending(v => v.CreatedAt)
            .FirstOrDefaultAsync(ct);

        return Results.Ok(new
        {
            Consultation = MapConsultation(consultation),
            ChatRoom = chatRoom is null ? null : new
            {
                chatRoom.Id,
                chatRoom.IsReadOnly,
                chatRoom.OpenedAtUtc,
                chatRoom.ClosedAtUtc
            },
            Prescription = prescription is null ? null : new
            {
                prescription.Id,
                prescription.PrescriptionNumber,
                prescription.ClinicalDiagnosisSummary,
                prescription.PrescribedHerbsJson,
                prescription.DrugHerbInteractionWarnings,
                prescription.DietaryAndLifestyleInstructions,
                prescription.DurationDays,
                prescription.PdfDocumentUrl,
                prescription.IssuedAtUtc
            },
            VideoSession = latestVideoSession is null ? null : new
            {
                latestVideoSession.Id,
                ProviderType = latestVideoSession.ProviderType.ToString(),
                latestVideoSession.ExternalRoomName,
                SessionStatus = latestVideoSession.SessionStatus.ToString(),
                latestVideoSession.StartedAtUtc,
                latestVideoSession.EndedAtUtc,
                latestVideoSession.DurationSeconds
            }
        });
    }

    private static async Task<IResult> StartVideo(
        Guid id,
        ICurrentUserService currentUser,
        TelePharmacyDbContext db,
        IVideoProvider videoProvider,
        IHubContext<ConsultationHub> hubContext,
        CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Results.Unauthorized();

        var profile = await db.PharmacistProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == currentUser.UserId.Value && !p.IsDeleted, ct);

        if (profile is null)
            return Results.NotFound(new { message = "Pharmacist profile not found." });

        var consultation = await db.Consultations
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);

        if (consultation is null)
            return Results.NotFound(new { message = "Consultation not found." });

        if (consultation.PharmacistProfileId != profile.Id)
            return Results.Forbid();

        if (consultation.Status != ConsultationStatus.ActiveChat &&
            consultation.Status != ConsultationStatus.InVideoCall)
        {
            return Results.Conflict(new
            {
                message = $"Cannot start video call in consultation status '{consultation.Status}'. Must be ActiveChat or InVideoCall."
            });
        }

        var roomName = await videoProvider.CreateRoomAsync($"consultation-{consultation.Id:N}", ct);
        var pharmacistToken = await videoProvider.GenerateTokenAsync(
            roomName, currentUser.UserId.Value.ToString(), ct);

        var videoSession = VideoSession.Create(
            consultation.Id,
            currentUser.UserId.Value,
            VideoProviderType.Jitsi,
            roomName);

        videoSession.Start();
        db.VideoSessions.Add(videoSession);

        consultation.StartVideo();
        await db.SaveChangesAsync(ct);

        var chatRoom = await db.ChatRooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.ConsultationId == consultation.Id && !r.IsDeleted, ct);

        var videoEvent = new
        {
            ConsultationId = consultation.Id,
            VideoSessionId = videoSession.Id,
            RoomName = roomName,
            Provider = VideoProviderType.Jitsi.ToString(),
            videoSession.StartedAtUtc
        };

        if (chatRoom is not null)
        {
            await hubContext.Clients.Group(chatRoom.Id.ToString())
                .SendAsync("VideoCallStarted", videoEvent, ct);

            await hubContext.Clients.Group(chatRoom.Id.ToString())
                .SendAsync("IncomingVideoCall", videoEvent, ct);
        }

        return Results.Ok(new
        {
            VideoSessionId = videoSession.Id,
            ConsultationId = consultation.Id,
            RoomName = roomName,
            Token = pharmacistToken,
            Provider = VideoProviderType.Jitsi.ToString(),
            Status = VideoSessionStatus.Active.ToString(),
            videoSession.StartedAtUtc
        });
    }

    private static async Task<IResult> EndVideo(
        Guid id,
        ICurrentUserService currentUser,
        TelePharmacyDbContext db,
        IVideoProvider videoProvider,
        CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Results.Unauthorized();

        var profile = await db.PharmacistProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == currentUser.UserId.Value && !p.IsDeleted, ct);

        if (profile is null)
            return Results.NotFound(new { message = "Pharmacist profile not found." });

        var consultation = await db.Consultations
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);

        if (consultation is null)
            return Results.NotFound(new { message = "Consultation not found." });

        if (consultation.PharmacistProfileId != profile.Id)
            return Results.Forbid();

        var activeVideoSession = await db.VideoSessions
            .Where(v => v.ConsultationId == consultation.Id &&
                        v.SessionStatus == VideoSessionStatus.Active &&
                        !v.IsDeleted)
            .OrderByDescending(v => v.StartedAtUtc)
            .FirstOrDefaultAsync(ct);

        if (activeVideoSession is not null)
        {
            activeVideoSession.End();
            await videoProvider.EndRoomAsync(activeVideoSession.ExternalRoomName, ct);
        }

        consultation.ReturnToChat();
        await db.SaveChangesAsync(ct);

        return Results.Ok(new
        {
            ConsultationId = consultation.Id,
            Status = consultation.Status.ToString(),
            VideoSessionId = activeVideoSession?.Id,
            DurationSeconds = activeVideoSession?.DurationSeconds
        });
    }

    private static async Task<IResult> IssueHerbalPrescription(
        Guid id,
        IssueHerbalPrescriptionRequest request,
        ICurrentUserService currentUser,
        TelePharmacyDbContext db,
        IPublisher publisher,
        IHubContext<ConsultationHub> hubContext,
        CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Results.Unauthorized();

        var profile = await db.PharmacistProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == currentUser.UserId.Value && !p.IsDeleted, ct);

        if (profile is null)
            return Results.NotFound(new { message = "Pharmacist profile not found." });

        var consultation = await db.Consultations
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);

        if (consultation is null)
            return Results.NotFound(new { message = "Consultation not found." });

        if (consultation.PharmacistProfileId != profile.Id)
            return Results.Forbid();

        if (consultation.Status != ConsultationStatus.ActiveChat &&
            consultation.Status != ConsultationStatus.InVideoCall)
        {
            return Results.Conflict(new
            {
                message = $"Cannot issue herbal prescription for consultation in status '{consultation.Status}'. Must be ActiveChat or InVideoCall."
            });
        }

        if (request.DurationDays <= 0)
            return Results.BadRequest(new { message = "DurationDays must be greater than zero." });

        string herbsJson;
        if (request.Herbs is not null && request.Herbs.Count > 0)
        {
            herbsJson = JsonSerializer.Serialize(request.Herbs);
        }
        else if (!string.IsNullOrWhiteSpace(request.PrescribedHerbsJson))
        {
            try
            {
                using var doc = JsonDocument.Parse(request.PrescribedHerbsJson);
                herbsJson = request.PrescribedHerbsJson;
            }
            catch (JsonException)
            {
                return Results.BadRequest(new { message = "PrescribedHerbsJson is not valid JSON." });
            }
        }
        else
        {
            return Results.BadRequest(new { message = "Prescribed herbs must be provided." });
        }

        var prescriptionNumber = $"RX-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..20].ToUpperInvariant();

        var prescription = HerbalPrescription.Create(
            consultation.Id,
            consultation.PatientId,
            profile.Id,
            prescriptionNumber,
            request.ClinicalDiagnosisSummary,
            herbsJson,
            request.DrugHerbInteractionWarnings,
            request.DietaryAndLifestyleInstructions,
            request.DurationDays,
            request.PdfDocumentUrl);

        db.HerbalPrescriptions.Add(prescription);

        var chatRoom = await db.ChatRooms
            .FirstOrDefaultAsync(r => r.ConsultationId == consultation.Id && !r.IsDeleted, ct);

        chatRoom?.Close();
        consultation.Complete();

        await db.SaveChangesAsync(ct);

        var integrationEvent = new HerbalPrescriptionIssuedIntegrationEvent(
            prescription.Id,
            consultation.Id,
            consultation.PatientId,
            profile.Id,
            prescriptionNumber,
            herbsJson,
            new DateTimeOffset(prescription.IssuedAtUtc, TimeSpan.Zero));

        await publisher.Publish(integrationEvent, ct);

        if (chatRoom is not null)
        {
            await hubContext.Clients.Group(chatRoom.Id.ToString()).SendAsync("PrescriptionIssued", new
            {
                PrescriptionId = prescription.Id,
                PrescriptionNumber = prescription.PrescriptionNumber,
                ConsultationId = consultation.Id,
                IssuedAtUtc = prescription.IssuedAtUtc,
                IsChatClosed = true
            }, ct);
        }

        return Results.Ok(new
        {
            PrescriptionId = prescription.Id,
            PrescriptionNumber = prescription.PrescriptionNumber,
            ConsultationId = consultation.Id,
            PatientId = consultation.PatientId,
            PharmacistProfileId = profile.Id,
            prescription.ClinicalDiagnosisSummary,
            PrescribedHerbsJson = herbsJson,
            prescription.DrugHerbInteractionWarnings,
            prescription.DietaryAndLifestyleInstructions,
            prescription.DurationDays,
            prescription.PdfDocumentUrl,
            prescription.IssuedAtUtc,
            ConsultationStatus = consultation.Status.ToString(),
            ChatRoomClosed = chatRoom?.IsReadOnly ?? true
        });
    }

    private static async Task<IResult> GetConsultationMessages(
        Guid id,
        [AsParameters] PagedRequest pagedRequest,
        TelePharmacyDbContext db,
        CancellationToken ct)
    {
        var chatRoom = await db.ChatRooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.ConsultationId == id && !r.IsDeleted, ct);

        if (chatRoom is null)
            return Results.NotFound(new { message = "Chat room not found for this consultation." });

        var messages = await db.ChatMessages
            .AsNoTracking()
            .Where(m => m.ChatRoomId == chatRoom.Id && !m.IsDeleted)
            .OrderBy(m => m.SentAtUtc)
            .Select(m => new ChatMessageResponse(
                m.Id,
                m.ChatRoomId,
                m.SenderUserId,
                m.SenderRole.ToString(),
                m.MessageType.ToString(),
                m.MessageContent,
                m.AttachmentUrl,
                m.IsRead,
                m.SentAtUtc))
            .ToPagedResultAsync(pagedRequest, ct);

        return Results.Ok(messages);
    }

    private static ConsultationResponse MapConsultation(Consultation c) => new(
        c.Id,
        c.PatientId,
        c.PharmacistProfileId,
        c.TenantId,
        c.AgreedFeeAmount,
        c.ChiefComplaintAndSymptoms,
        c.ClinicalSummaryConsentGranted,
        c.Status.ToString(),
        c.RejectionReason,
        c.RequestedAtUtc,
        c.ExpiresAtUtc,
        c.AcceptedAtUtc,
        c.CompletedAtUtc);

    public sealed record CreateConsultationRequest(
        Guid PharmacistProfileId,
        string ChiefComplaintAndSymptoms,
        bool ClinicalSummaryConsentGranted,
        Guid? PatientId = null,
        int? DurationHours = null);

    public sealed record AcceptConsultationRequest(decimal? Fee = null);

    public sealed record RejectConsultationRequest(string RejectionReason);

    public sealed record IssueHerbalPrescriptionRequest(
        string? ClinicalDiagnosisSummary,
        List<PrescribedHerbItem>? Herbs,
        string? PrescribedHerbsJson,
        string? DrugHerbInteractionWarnings,
        string? DietaryAndLifestyleInstructions,
        int DurationDays = 30,
        string? PdfDocumentUrl = null);

    public sealed record PrescribedHerbItem(
        Guid? HerbId,
        string HerbName,
        string Dosage,
        string PreparationInstructions,
        string? Frequency = null,
        int? DurationDays = null);

    public sealed record ConsultationResponse(
        Guid Id,
        Guid PatientId,
        Guid PharmacistProfileId,
        Guid? TenantId,
        decimal AgreedFeeAmount,
        string ChiefComplaintAndSymptoms,
        bool ClinicalSummaryConsentGranted,
        string Status,
        string? RejectionReason,
        DateTime RequestedAtUtc,
        DateTime ExpiresAtUtc,
        DateTime? AcceptedAtUtc,
        DateTime? CompletedAtUtc);

    public sealed record ChatMessageResponse(
        Guid Id,
        Guid ChatRoomId,
        Guid SenderUserId,
        string SenderRole,
        string MessageType,
        string? MessageContent,
        string? AttachmentUrl,
        bool IsRead,
        DateTime SentAtUtc);
}
