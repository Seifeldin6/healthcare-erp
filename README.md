# Healthcare ERP

A modular monolith healthcare platform designed for hospital management, tele-pharmacy consultations, herbal/complementary prescriptions, patient Electronic Health Records (EHR), and real-time telehealth video calling.

---

## 1. Architecture Overview

Healthcare ERP follows a Modular Monolith architecture pattern with Vertical Slice Architecture inside each module:

```
healthcare-erp/
├── backend/
│   ├── src/
│   │   ├── Erp.Api/                 # Minimal APIs host, Swagger, JWT Auth, SignalR Hubs
│   │   ├── Erp.SharedKernel/        # Domain primitives, common interfaces, pagination, events
│   │   ├── Modules/
│   │   │   ├── Erp.Modules.Core/    # Identity, Multi-Tenancy, Global Patient EHR, Audit Logs
│   │   │   └── Erp.Modules.TelePharmacy/ # Consultations, Chat Rooms, Video Sessions, Herbal Catalog & Rx
│   │   └── Erp.slnx                 # .NET 9 Solution file
│   └── tests/
│       └── Erp.Tests/               # Automated integration tests (xUnit + WebApplicationFactory)
├── docs/                            # ERD, Architecture, Deployment & API Integration Guides
└── rules.md                         # Architecture guidelines & engineering constraints
```

### Module Isolation & Database Boundaries
- **PostgreSQL Isolated Schemas**: All modules share a single PostgreSQL database instance but write strictly to their own dedicated schema:
  - `core`: Tenants, Users, Patients, Clinical Summaries, Payments, Audit Logs.
  - `telepharmacy`: Pharmacist Profiles, Herbs Catalog, Consultations, Chat Rooms, Chat Messages, Video Sessions, Herbal Prescriptions.
- **Cross-Module Communication**: No cross-module database joins or direct DbContext references. Inter-module communication occurs exclusively via immutable in-process integration events dispatched through MediatR (`ConsultationPaidIntegrationEvent`, `HerbalPrescriptionIssuedIntegrationEvent`).

---

## 2. Technology Stack

- **Framework**: .NET 9 Web API (Minimal APIs)
- **Data Access**: Entity Framework Core 9 with Npgsql PostgreSQL Provider
- **Database**: PostgreSQL 16+ (PostgreSQL 18 tested)
- **Messaging & In-Process Events**: MediatR 12
- **Real-Time WebSockets**: ASP.NET Core SignalR
- **Validation**: FluentValidation
- **Authentication**: JWT Bearer with custom claims (`sub`, `tenant_id`, `role`, `patient_id`)
- **API Documentation**: OpenAPI / Swagger UI (Swashbuckle 10)
- **Telehealth Video**: Jitsi Meet WebRTC integration (configurable via `IVideoProvider`)
- **Testing**: xUnit 2.9, Microsoft.AspNetCore.Mvc.Testing

---

## 3. Getting Started & Local Setup

### 3.1 Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (v9.0.100 or newer)
- [PostgreSQL](https://www.postgresql.org/) (running locally on port 5432)

### 3.2 Database Initialization
Ensure PostgreSQL is running and a database named `HealthcareErp` exists:

```bash
# Example PostgreSQL database creation
psql -h localhost -U postgres -c "CREATE DATABASE \"HealthcareErp\";"
```

The database connection string in `backend/src/Erp.Api/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=HealthcareErp;Username=postgres;Password=postgres"
}
```

### 3.3 Running the Application
From the repository root:

```bash
dotnet run --project backend/src/Erp.Api/Erp.Api.csproj
```

In Development mode, the application automatically:
1. Applies pending EF Core database migrations for both `core` and `telepharmacy` schemas.
2. Seeds initial test tenants, pharmacists, patients, and herbs idempotently.
3. Serves Swagger UI at `http://localhost:5000/swagger` (Root `/` redirects directly to `/swagger`).
4. Exposes the SignalR real-time hub at `ws://localhost:5000/hubs/consultation`.

### 3.4 Running Automated Integration Tests
Execute the entire end-to-end integration test suite:

```bash
dotnet test backend/src/Erp.slnx -v normal
```

---

## 4. Seeded Test Credentials

The development database seeds verified accounts upon startup:

| Role | Email | Password | Details |
|---|---|---|---|
| Pharmacist | `pharmacist@erp.local` | `Password123!` | Specialized in Herbal & Complementary Medicine. Approved, fee 150.00 EGP. |
| Patient | `patient@erp.local` | `Password123!` | Complete clinical profile with seeded allergies, chronic conditions, and meds. |

---

## 5. Documentation Directory

- [Architecture Guide](docs/Architecture.md): Modular Monolith design, sequence diagrams, and module boundaries.
- [Database Schema (ERD)](docs/ERD.md): Entity-Relationship diagrams and schema definitions.
- [API Integration Guide](docs/API_Integration_Guide.md): Complete REST endpoint reference, JSON payloads, SignalR Hub methods/events, and React/Flutter SDK examples.
- [Docker Deployment Guide](docs/DOCKER_DEPLOYMENT_GUIDE.md): Multi-stage Docker build, environment variables, Nginx reverse proxy configuration with WebSocket support, and Jitsi VPS hosting setup.
