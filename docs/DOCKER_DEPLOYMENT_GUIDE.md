# Healthcare ERP - Production Docker & Deployment Guide

This document provides production deployment, containerization, reverse proxy, and video server hosting specifications for DevOps and infrastructure engineers.

---

## 1. Runtime & Solution Specifications

- **Target Framework**: .NET 9.0 (`net9.0`)
- **Base Images**:
  - Build Stage: `mcr.microsoft.com/dotnet/sdk:9.0-alpine`
  - Runtime Stage: `mcr.microsoft.com/dotnet/aspnet:9.0-alpine`
- **Solution File**: `backend/src/Erp.slnx`
- **Startup Project**: `backend/src/Erp.Api/Erp.Api.csproj`
- **Output Assembly**: `Erp.Api.dll`

---

## 2. Database Requirements

- **Engine**: PostgreSQL 18+ (PostgreSQL 18.6 tested and verified)
- **Database Architecture**: Single database instance (e.g. `healthcare_erp`) with two isolated schemas:
  - `core`
  - `telepharmacy`
- **Migrations & Seeding**:
  - In `Development` mode, EF Core migrations and test seeder execution run automatically at startup.
  - In `Production` mode, migrations are executed either as part of a pre-deployment migration step (`dotnet ef database update`) or controlled container startup.

---

## 3. Environment Variables Reference

Production deployments override configuration using standard ASP.NET Core double-underscore (`__`) environment variables:

| Environment Variable | Description | Required in Production | Default / Example Value |
|---|---|---|---|
| `ASPNETCORE_ENVIRONMENT` | Hosting environment (`Production`, `Staging`, `Development`) | Yes | `Production` |
| `ASPNETCORE_URLS` | Kestrel binding URLs | Yes | `http://+:5000` |
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string | Yes | `Host=postgres;Port=5432;Database=healthcare_erp;Username=erp_app;Password=<STRONG_PASSWORD>` |
| `Jwt__Key` | Secret key for HMAC-SHA256 signing (minimum 32 characters) | Yes | `<STRONG_CRYPTOGRAPHIC_KEY_32_BYTES_MIN>` |
| `Jwt__Issuer` | JWT issuer validation claim | Yes | `healthcare-erp-api` |
| `Jwt__Audience` | JWT audience validation claim | Yes | `healthcare-erp-clients` |
| `Video__ServerUrl` | Jitsi Meet server base URL | Yes | `https://meet.jit.si` or `https://jitsi.yourdomain.com` |
| `Jitsi__AppId` | Optional Jitsi JWT application identifier | No | `healthcare-erp` |
| `Jitsi__AppSecret` | Optional Jitsi JWT secret key for private authentication | No | `<OPTIONAL_SECRET>` |

---

## 4. Reverse Proxy Configuration (Nginx & Traefik)

The API hosts both REST Minimal APIs and real-time WebSockets (SignalR) at `/hubs/consultation`. The reverse proxy MUST forward HTTP/1.1 WebSocket headers.

### 4.1 Nginx Reference Configuration

```nginx
upstream erp_backend {
    server erp-api:5000;
    keepalive 32;
}

server {
    listen 80;
    server_name api.healthcare.local;
    return 301 https://$host$request_uri;
}

server {
    listen 443 ssl http2;
    server_name api.healthcare.local;

    ssl_certificate /etc/ssl/certs/api.healthcare.local.crt;
    ssl_certificate_key /etc/ssl/private/api.healthcare.local.key;
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers HIGH:!aNULL:!MD5;

    # General REST API proxy
    location / {
        proxy_pass http://erp_backend;
        proxy_http_version 1.1;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    # SignalR Real-Time WebSocket Hub (Requires Upgrade headers)
    location /hubs/ {
        proxy_pass http://erp_backend;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection "upgrade";
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;

        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;

        # WebSocket timeouts
        proxy_read_timeout 3600s;
        proxy_send_timeout 3600s;
    }
}
```

---

## 5. Telehealth Video Server Hosting Options

### Option A: Public Jitsi Cloud (Default / Zero Maintenance)
- Configuration: Set `Video__ServerUrl=https://meet.jit.si`.
- No additional server infrastructure or open ports required.
- Ideal for development, staging, or low-cost deployments.

### Option B: Self-Hosted Jitsi Meet on Private Linux VPS (HIPAA / Data Privacy)
- Host on an isolated Ubuntu / Debian VPS with Docker and Docker Compose.
- **Network Ports Required**:
  - `80/TCP`: HTTP web interface and Let's Encrypt certificate renewal.
  - `443/TCP`: HTTPS web interface and WebSocket signaling.
  - `10000/UDP`: Jitsi Videobridge (JVB) media streaming (audio & video RTP).
- **Setup Procedure**:
  1. Clone official Jitsi Docker repository:
     ```bash
     git clone https://github.com/jitsi/docker-jitsi-meet && cd docker-jitsi-meet
     cp env.example .env
     ./gen-passwords.sh
     ```
  2. Configure `.env`:
     ```ini
     HTTP_PORT=80
     HTTPS_PORT=443
     PUBLIC_URL=https://jitsi.yourdomain.com
     DOCKER_HOST_ADDRESS=<VPS_PUBLIC_IP>
     ENABLE_AUTH=0
     ENABLE_GUESTS=1
     ```
  3. Start Jitsi stack:
     ```bash
     docker compose up -d
     ```
  4. In Healthcare ERP environment variables, update:
     ```ini
     Video__ServerUrl=https://jitsi.yourdomain.com
     ```
  The API and mobile/web clients will automatically construct all meeting room links on `https://jitsi.yourdomain.com/<room-name>` without code modifications.

---

## 6. Multi-Stage Production Dockerfile

Reference `Dockerfile` placed at `backend/Dockerfile` or repository root:

```dockerfile
# Stage 1: Build & Publish
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /app

# Copy solution and project definitions for layer caching
COPY src/Erp.slnx ./src/
COPY src/Erp.Api/Erp.Api.csproj ./src/Erp.Api/
COPY src/Erp.SharedKernel/Erp.SharedKernel.csproj ./src/Erp.SharedKernel/
COPY src/Modules/Erp.Modules.Core/Erp.Modules.Core.csproj ./src/Modules/Erp.Modules.Core/
COPY src/Modules/Erp.Modules.TelePharmacy/Erp.Modules.TelePharmacy.csproj ./src/Modules/Erp.Modules.TelePharmacy/

# Restore dependencies
RUN dotnet restore src/Erp.Api/Erp.Api.csproj

# Copy all source code
COPY src/ ./src/

# Build and publish release binary
RUN dotnet publish src/Erp.Api/Erp.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# Stage 2: Minimal Alpine Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app

# Install curl for container health check
RUN apk add --no-cache curl tzdata

# Create non-root user for security
RUN addgroup -S appgroup && adduser -S appuser -G appgroup
USER appuser

COPY --from=build --chown=appuser:appgroup /app/publish .

ENV ASPNETCORE_URLS=http://+:5000 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 5000

HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
  CMD curl -f http://localhost:5000/swagger || exit 1

ENTRYPOINT ["dotnet", "Erp.Api.dll"]
```

---

## 7. Production Docker Compose Stack

Complete `docker-compose.yml` defining the API service and PostgreSQL with persistent storage and healthchecks:

```yaml
version: '3.8'

services:
  postgres:
    image: postgres:18-alpine
    container_name: erp_postgres
    restart: unless-stopped
    environment:
      POSTGRES_DB: healthcare_erp
      POSTGRES_USER: erp_admin
      POSTGRES_PASSWORD: StrongProductionPassword2026!
    volumes:
      - postgres_data:/var/lib/postgresql/data
    ports:
      - "5432:5432"
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U erp_admin -d healthcare_erp"]
      interval: 10s
      timeout: 5s
      retries: 5

  api:
    build:
      context: ./backend
      dockerfile: Dockerfile
    container_name: erp_api
    restart: unless-stopped
    depends_on:
      postgres:
        condition: service_healthy
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:5000
      - ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=healthcare_erp;Username=erp_admin;Password=StrongProductionPassword2026!
      - Jwt__Key=healthcare-erp-super-secret-jwt-key-2026!
      - Jwt__Issuer=healthcare-erp-api
      - Jwt__Audience=healthcare-erp-clients
      - Video__ServerUrl=https://meet.jit.si
    ports:
      - "5000:5000"

volumes:
  postgres_data:
    driver: local
```
