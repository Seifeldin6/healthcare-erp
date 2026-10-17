erDiagram
    CORE_TENANTS ||--o{ CORE_USERS : "employs_optional"
    CORE_USERS ||--o| CORE_PATIENTS : "has_patient_profile"
    CORE_PATIENTS ||--|| CORE_PATIENT_CLINICAL_SUMMARIES : "has_vital_summary"
    CORE_PATIENTS ||--o{ CORE_PAYMENTS : "makes_payment"
    CORE_USERS ||--o| TELEPHARMACY_PHARMACIST_PROFILES : "has_pharmacist_profile"
    CORE_TENANTS ||--o{ TELEPHARMACY_PHARMACIST_PROFILES : "affiliates_optional"

    CORE_PATIENTS ||--o{ TELEPHARMACY_CONSULTATIONS : "requests"
    TELEPHARMACY_PHARMACIST_PROFILES ||--o{ TELEPHARMACY_CONSULTATIONS : "handles"
    TELEPHARMACY_CONSULTATIONS ||--o| TELEPHARMACY_CHAT_ROOMS : "opens_room"
    TELEPHARMACY_CHAT_ROOMS ||--o{ TELEPHARMACY_CHAT_MESSAGES : "contains"
    CORE_USERS ||--o{ TELEPHARMACY_CHAT_MESSAGES : "sends"
    TELEPHARMACY_CONSULTATIONS ||--o{ TELEPHARMACY_VIDEO_SESSIONS : "triggers_call"
    TELEPHARMACY_CONSULTATIONS ||--o| TELEPHARMACY_HERBAL_PRESCRIPTIONS : "produces_rx"

    TELEPHARMACY_CONSULTATIONS ||..o| CORE_PAYMENTS : "polymorphic_payment_ref"
    CORE_USERS ||..o{ CORE_AUDIT_LOGS : "logged_in_audit"
    TELEPHARMACY_HERBS_CATALOG }o..o{ TELEPHARMACY_HERBAL_PRESCRIPTIONS : "snapshot_in_jsonb"

    CORE_TENANTS {
        uuid Id PK
        varchar Name
        varchar TenantType "Hospital | Clinic | Pharmacy"
        varchar SubscriptionPlan "Basic | Pro | Enterprise"
        jsonb EnabledModulesJson
        boolean IsActive
        boolean IsDeleted
        timestamptz SubscriptionEndsAtUtc
        timestamptz CreatedAtUtc
    }

    CORE_USERS {
        uuid Id PK
        uuid TenantId FK "Nullable for Global Patients and Freelance Pharmacists"
        varchar Email UK
        varchar PhoneNumber UK
        varchar PasswordHash
        varchar FullName
        varchar Role "SuperAdmin | TenantAdmin | Doctor | Pharmacist | Nurse | Patient"
        boolean IsActive
        boolean IsDeleted
        timestamptz CreatedAtUtc
    }

    CORE_PATIENTS {
        uuid Id PK
        uuid UserId FK "Unique Reference to CORE_USERS"
        varchar NationalId UK
        date DateOfBirth
        varchar Gender "Male | Female"
        varchar BloodType "A+ | A- | B+ | B- | AB+ | AB- | O+ | O- | Unknown"
        decimal WeightKg
        decimal HeightCm
        varchar EmergencyContactName
        varchar EmergencyContactPhone
        boolean IsDeleted
        timestamptz CreatedAtUtc
    }

    CORE_PATIENT_CLINICAL_SUMMARIES {
        uuid Id PK
        uuid PatientId FK "Unique Reference to CORE_PATIENTS"
        jsonb AllergiesJson "Array of drug, food, and herb allergies"
        jsonb ChronicConditionsJson "Array of chronic diseases"
        jsonb ActiveMedicationsJson "Array of current ongoing medications"
        jsonb ActiveHerbalPrescriptionsJson "Array of active herbal treatments"
        jsonb MajorSurgeriesJson "Array of past major surgeries"
        timestamptz LastUpdatedAtUtc
    }

    CORE_PAYMENTS {
        uuid Id PK
        uuid PatientId FK "References CORE_PATIENTS"
        uuid TenantId FK "Nullable if Freelance Pharmacist"
        uuid ReferenceEntityId "Loose Ref to ConsultationId or Future InvoiceId"
        varchar ReferenceModule "TelePharmacy | Inpatient | Outpatient"
        decimal Amount
        varchar Currency "EGP | USD"
        varchar GatewayProvider "MockGateway | Paymob | Stripe"
        varchar GatewayTransactionId
        varchar Status "Pending | Paid | Failed | Refunded"
        timestamptz CreatedAtUtc
        timestamptz PaidAtUtc
    }

    CORE_AUDIT_LOGS {
        uuid Id PK
        uuid UserId "Loose Reference to User or System"
        uuid TenantId "Nullable"
        varchar ModuleName
        varchar EntityName
        varchar EntityId
        varchar ActionType "Create | Update | SoftDelete | AccessClinicalSummary"
        jsonb OldValuesJson
        jsonb NewValuesJson
        timestamptz OccurredAtUtc
    }

    TELEPHARMACY_PHARMACIST_PROFILES {
        uuid Id PK
        uuid UserId FK "Unique Reference to CORE_USERS"
        uuid TenantId FK "Nullable for Independent Freelance Pharmacist"
        varchar SyndicateLicenseNumber UK
        varchar LicenseDocumentUrl
        varchar VerificationStatus "PendingReview | Approved | Rejected | Suspended"
        varchar Specialization "ClinicalPharmacy | HerbalAndComplementaryMedicine"
        varchar Bio
        decimal ConsultationFee
        boolean IsAvailableForRequests
        boolean IsDeleted
        timestamptz VerifiedAtUtc
    }

    TELEPHARMACY_HERBS_CATALOG {
        uuid Id PK
        varchar ScientificName UK
        varchar ArabicCommonName
        varchar EnglishCommonName
        varchar ActiveIngredients
        jsonb ContraindicationsJson "Drug and disease interactions warnings"
        varchar StandardPreparationForms "Infusion | Decoction | Tincture | Capsule | Topical"
        boolean IsApprovedForPrescription
        boolean IsDeleted
    }

    TELEPHARMACY_CONSULTATIONS {
        uuid Id PK
        uuid PatientId FK "References CORE_PATIENTS"
        uuid PharmacistProfileId FK "References TELEPHARMACY_PHARMACIST_PROFILES"
        uuid TenantId FK "Nullable if Freelance Pharmacist"
        decimal AgreedFeeAmount "Snapshot of fee at request time"
        text ChiefComplaintAndSymptoms
        boolean ClinicalSummaryConsentGranted
        varchar Status "Pending | Rejected | Expired | AcceptedAwaitingPayment | ActiveChat | InVideoCall | Completed"
        text RejectionReason
        boolean IsDeleted
        timestamptz RequestedAtUtc
        timestamptz ExpiresAtUtc
        timestamptz AcceptedAtUtc
        timestamptz CompletedAtUtc
    }

    TELEPHARMACY_CHAT_ROOMS {
        uuid Id PK
        uuid ConsultationId FK "Unique Reference to TELEPHARMACY_CONSULTATIONS"
        boolean IsReadOnly "True immediately after Consultation is Completed"
        timestamptz OpenedAtUtc
        timestamptz ClosedAtUtc
    }

    TELEPHARMACY_CHAT_MESSAGES {
        uuid Id PK
        uuid ChatRoomId FK
        uuid SenderUserId FK
        varchar SenderRole "Patient | Pharmacist | System"
        varchar MessageType "Text | ImageAttachment | LabReportPdf | SystemNotice"
        text MessageContent
        varchar AttachmentUrl
        boolean IsRead
        timestamptz SentAtUtc
    }

    TELEPHARMACY_VIDEO_SESSIONS {
        uuid Id PK
        uuid ConsultationId FK
        uuid TriggeredByPharmacistUserId FK "Enforces Pharmacist-Only Trigger Rule"
        varchar ProviderType "Jitsi | Agora | CustomWebRtc"
        varchar ExternalRoomName
        varchar SessionStatus "Ringing | Active | Ended | Missed"
        timestamptz StartedAtUtc
        timestamptz EndedAtUtc
        int DurationSeconds
    }

    TELEPHARMACY_HERBAL_PRESCRIPTIONS {
        uuid Id PK
        uuid ConsultationId FK "Unique Reference to TELEPHARMACY_CONSULTATIONS"
        uuid PatientId FK
        uuid PharmacistProfileId FK
        varchar PrescriptionNumber UK
        text ClinicalDiagnosisSummary
        jsonb PrescribedHerbsJson "Snapshot array of herbs from Catalog + dosage and prep"
        text DrugHerbInteractionWarnings
        text DietaryAndLifestyleInstructions
        integer DurationDays
        varchar PdfDocumentUrl
        boolean IsDeleted
        timestamptz IssuedAtUtc
    }