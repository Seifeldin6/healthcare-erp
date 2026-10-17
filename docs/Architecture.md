# Healthcare ERP - System Architecture & Workflows

## 1. High-Level System & Modular Monolith Architecture

```mermaid
graph TB
    subgraph Clients ["Client Applications"]
        MobileApp["Flutter Mobile App<br/>(Unified Patient Application)"]
        WebApp["React Web Portal<br/>(Pharmacist & Hospital SaaS Portal)"]
    end

    subgraph Backend [".NET 9 Backend (Modular Monolith)"]
        APIGateway["API Layer & SignalR Hubs<br/>(Minimal APIs + JWT Auth + Real-Time)"]

        subgraph Shared ["Shared Kernel"]
            EventBus["In-Memory Event Bus (MediatR)"]
            Interceptors["Audit Trail & Multi-Tenant Filters"]
            Interfaces["Adapters Interfaces<br/>(IVideoProvider, IPaymentGateway, IRegulatory)"]
        end

        subgraph ActiveModules ["Active Modules (MVP Phase 1)"]
            CoreMod["Core Module<br/>(IAM, Tenants/SaaS, Global Patient EHR)"]
            TelePharmMod["Tele-Pharmacy Module<br/>(Consultations, Chat, Video, Herbal Rx)"]
        end

        subgraph FutureModules ["Future Modules (Phase 2 & 3 - Plug & Play)"]
            InpatientMod["Inpatient & Nursing Module<br/>(Beds/ADT, Vitals, eMAR, Handover)"]
            InventoryMod["Pharmacy & Inventory Module<br/>(Stock, Waste, Drug Schedules)"]
            FinanceHRMod["HR & Billing Module<br/>(Shifts, Invoices, Insurance)"]
        end
    end

    subgraph ExternalAdapters ["External Providers (Pluggable)"]
        VideoSDK["Video Provider<br/>(Agora / Jitsi now -> WebRTC later)"]
        PaymentGW["Payment Provider<br/>(Mock Gateway now -> Paymob later)"]
        EDA["Regulatory System<br/>(EDA Integration - Future Adapter)"]
    end

    subgraph Database ["PostgreSQL Database (Single DB, Isolated Schemas)"]
        CoreSchema[("Schema: core<br/>Users, Tenants, Patients")]
        TeleSchema[("Schema: telepharmacy<br/>Requests, Chats, HerbalRx")]
        InpatientSchema[("Schema: inpatient<br/>Rooms, Vitals, eMAR")]
        InventorySchema[("Schema: inventory<br/>Drugs, Batches")]
    end

    MobileApp <==>|"REST API + SignalR"| APIGateway
    WebApp <==>|"REST API + SignalR"| APIGateway
    APIGateway --> CoreMod
    APIGateway --> TelePharmMod
    CoreMod <--> EventBus
    TelePharmMod <--> EventBus
    TelePharmMod --> Interfaces
    Interfaces --> VideoSDK
    Interfaces --> PaymentGW
    CoreMod ==> CoreSchema
    TelePharmMod ==> TeleSchema
```

## 2. Tele-Pharmacy End-to-End Sequence Flow

```mermaid
sequenceDiagram
    autonumber
    actor Patient as Patient (Flutter)
    participant API as .NET Backend & SignalR
    participant DB as PostgreSQL
    actor Pharm as Pharmacist (React Web)

    Patient->>API: GET /api/telepharmacy/pharmacists (Available & Licensed)
    API-->>Patient: Return approved pharmacists list
    Patient->>API: POST /api/telepharmacy/consultations (Symptoms + Clinical Summary Consent)
    API->>DB: Save request with status (Pending)
    API-->>Pharm: SignalR Notification (New Consultation Request + Patient Clinical Summary)

    Pharm->>API: GET /api/core/patients/{id}/clinical-summary (Read Allergies & Chronic Meds)
    Pharm->>API: POST /api/telepharmacy/consultations/{id}/accept (Accept Request)
    API->>DB: Update status to (AcceptedAwaitingPayment)
    API-->>Patient: SignalR Notification (Request Accepted, Proceed to Payment)
    Patient->>API: POST /api/core/payments/mock-pay (Mock Payment Confirmation)
    API->>DB: Record Mock Transaction + Update status to (ActiveChat) + Create ChatRoom

    API-->>Patient: Open Chat Room
    API-->>Pharm: Open Chat Room
    Patient->>API: SignalR: SendMessage
    API-->>Pharm: SignalR: ReceiveMessage

    Pharm->>API: POST /api/telepharmacy/consultations/{id}/start-video (Role: Pharmacist Only)
    API->>API: Generate RoomId & Token via IVideoProvider
    API->>DB: Update status to (InVideoCall)
    API-->>Pharm: Return Room Config & Open Web Video
    API-->>Patient: SignalR Event: IncomingVideoCall (Enable Join Call Button for Patient)
    Patient->>API: Join Video Call

    Pharm->>API: POST /api/telepharmacy/consultations/{id}/herbal-prescription
    API->>DB: Save Prescription (JSONB) + Link to Global Patient EHR + Set status to (Completed / Read-Only)
    API-->>Patient: SignalR Notification (Prescription Issued & Chat set to Read-Only)
```

## 3. Consultation State Machine

```mermaid
stateDiagram-v2
    [*] --> Pending : Patient submits request
    Pending --> Rejected : Pharmacist rejects request
    Pending --> Expired : Timeout without response
    Pending --> AcceptedAwaitingPayment : Pharmacist accepts request
    AcceptedAwaitingPayment --> ActiveChat : Patient completes mock payment (Chat opens)
    ActiveChat --> InVideoCall : Pharmacist triggers Start Video (Join enabled for Patient)
    InVideoCall --> ActiveChat : Video call ends (Return to active chat)
    ActiveChat --> Completed : Pharmacist issues Herbal Prescription & closes session
    Rejected --> [*]
    Expired --> [*]
    Completed --> [*] : Chat becomes Read-Only & Prescription saved to EHR
```

## 4. Data Privacy & Multi-Tenancy Access Rules

```mermaid
graph LR
    subgraph GlobalData ["Global Patient Data (Core Schema)"]
        MasterProfile["Unified Patient Profile"]
        VitalHistory["Clinical Summary (Allergies, Chronic Conditions, Active Meds, Active Herbal Rx)"]
    end
    subgraph HospitalA ["Hospital A Data (Tenant A)"]
        EncA["Local Encounters & Billing"]
    end
    subgraph FreelancePharm ["Independent Pharmacist (TelePharmacy)"]
        PharmSession["Chat Logs, Video Logs & Herbal Session"]
    end
    PatientApp["Patient App (Flutter)"] ==>|Full Read Access| GlobalData
    PatientApp ==>|Reads History| HospitalA
    PatientApp ==>|Reads Consultations| FreelancePharm
    HospAPortal["Hospital A Web Portal"] -->|Read & Write| HospitalA
    HospAPortal -.->|Read-Only| VitalHistory
    PharmPortal["Pharmacist Web Portal"] -->|Read & Write| FreelancePharm
    PharmPortal -.->|Read-Only| VitalHistory
    PharmPortal x--x|Access Denied| HospitalA
```
