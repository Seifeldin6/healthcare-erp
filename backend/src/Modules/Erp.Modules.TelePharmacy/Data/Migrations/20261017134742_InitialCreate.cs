using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erp.Modules.TelePharmacy.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "telepharmacy");

            migrationBuilder.CreateTable(
                name: "chat_messages",
                schema: "telepharmacy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatRoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderRole = table.Column<string>(type: "text", nullable: false),
                    MessageType = table.Column<string>(type: "text", nullable: false),
                    MessageContent = table.Column<string>(type: "text", nullable: true),
                    AttachmentUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    SentAtUtc = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "chat_rooms",
                schema: "telepharmacy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsultationId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "boolean", nullable: false),
                    OpenedAtUtc = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    ClosedAtUtc = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "consultations",
                schema: "telepharmacy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: false),
                    PharmacistProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    AgreedFeeAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ChiefComplaintAndSymptoms = table.Column<string>(type: "text", nullable: false),
                    ClinicalSummaryConsentGranted = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    RejectionReason = table.Column<string>(type: "text", nullable: true),
                    RequestedAtUtc = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    AcceptedAtUtc = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    CompletedAtUtc = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consultations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "herbal_prescriptions",
                schema: "telepharmacy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsultationId = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: false),
                    PharmacistProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    PrescriptionNumber = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ClinicalDiagnosisSummary = table.Column<string>(type: "text", nullable: true),
                    PrescribedHerbsJson = table.Column<string>(type: "jsonb", nullable: false),
                    DrugHerbInteractionWarnings = table.Column<string>(type: "text", nullable: true),
                    DietaryAndLifestyleInstructions = table.Column<string>(type: "text", nullable: true),
                    DurationDays = table.Column<int>(type: "integer", nullable: false),
                    PdfDocumentUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    IssuedAtUtc = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_herbal_prescriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "herbs_catalog",
                schema: "telepharmacy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ScientificName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    ArabicCommonName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    EnglishCommonName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ActiveIngredients = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ContraindicationsJson = table.Column<string>(type: "jsonb", nullable: false),
                    StandardPreparationForms = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    IsApprovedForPrescription = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_herbs_catalog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "pharmacist_profiles",
                schema: "telepharmacy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    SyndicateLicenseNumber = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LicenseDocumentUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    VerificationStatus = table.Column<string>(type: "text", nullable: false),
                    Specialization = table.Column<string>(type: "text", nullable: false),
                    Bio = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ConsultationFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsAvailableForRequests = table.Column<bool>(type: "boolean", nullable: false),
                    VerifiedAtUtc = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pharmacist_profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "video_sessions",
                schema: "telepharmacy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsultationId = table.Column<Guid>(type: "uuid", nullable: false),
                    TriggeredByPharmacistUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProviderType = table.Column<string>(type: "text", nullable: false),
                    ExternalRoomName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    SessionStatus = table.Column<string>(type: "text", nullable: false),
                    StartedAtUtc = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    EndedAtUtc = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    DurationSeconds = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_video_sessions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_ChatRoomId",
                schema: "telepharmacy",
                table: "chat_messages",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_SenderUserId",
                schema: "telepharmacy",
                table: "chat_messages",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_chat_rooms_ConsultationId",
                schema: "telepharmacy",
                table: "chat_rooms",
                column: "ConsultationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consultations_PatientId",
                schema: "telepharmacy",
                table: "consultations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_consultations_PharmacistProfileId",
                schema: "telepharmacy",
                table: "consultations",
                column: "PharmacistProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_consultations_TenantId",
                schema: "telepharmacy",
                table: "consultations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_herbal_prescriptions_ConsultationId",
                schema: "telepharmacy",
                table: "herbal_prescriptions",
                column: "ConsultationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_herbal_prescriptions_PatientId",
                schema: "telepharmacy",
                table: "herbal_prescriptions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_herbal_prescriptions_PharmacistProfileId",
                schema: "telepharmacy",
                table: "herbal_prescriptions",
                column: "PharmacistProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_herbal_prescriptions_PrescriptionNumber",
                schema: "telepharmacy",
                table: "herbal_prescriptions",
                column: "PrescriptionNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_herbs_catalog_ScientificName",
                schema: "telepharmacy",
                table: "herbs_catalog",
                column: "ScientificName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pharmacist_profiles_SyndicateLicenseNumber",
                schema: "telepharmacy",
                table: "pharmacist_profiles",
                column: "SyndicateLicenseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pharmacist_profiles_TenantId",
                schema: "telepharmacy",
                table: "pharmacist_profiles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_pharmacist_profiles_UserId",
                schema: "telepharmacy",
                table: "pharmacist_profiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_video_sessions_ConsultationId",
                schema: "telepharmacy",
                table: "video_sessions",
                column: "ConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_video_sessions_TriggeredByPharmacistUserId",
                schema: "telepharmacy",
                table: "video_sessions",
                column: "TriggeredByPharmacistUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_messages",
                schema: "telepharmacy");

            migrationBuilder.DropTable(
                name: "chat_rooms",
                schema: "telepharmacy");

            migrationBuilder.DropTable(
                name: "consultations",
                schema: "telepharmacy");

            migrationBuilder.DropTable(
                name: "herbal_prescriptions",
                schema: "telepharmacy");

            migrationBuilder.DropTable(
                name: "herbs_catalog",
                schema: "telepharmacy");

            migrationBuilder.DropTable(
                name: "pharmacist_profiles",
                schema: "telepharmacy");

            migrationBuilder.DropTable(
                name: "video_sessions",
                schema: "telepharmacy");
        }
    }
}
