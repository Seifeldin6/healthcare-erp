# Stage 1: Build & Publish
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /app

# Copy solution and project files
COPY src/Erp.slnx ./src/
COPY src/Erp.Api/Erp.Api.csproj ./src/Erp.Api/
COPY src/Erp.SharedKernel/Erp.SharedKernel.csproj ./src/Erp.SharedKernel/
COPY src/Modules/Erp.Modules.Core/Erp.Modules.Core.csproj ./src/Modules/Erp.Modules.Core/
COPY src/Modules/Erp.Modules.TelePharmacy/Erp.Modules.TelePharmacy.csproj ./src/Modules/Erp.Modules.TelePharmacy/

# Restore dependencies
RUN dotnet restore src/Erp.Api/Erp.Api.csproj

# Copy all source files
COPY src/ ./src/

# Build and publish release binary
RUN dotnet publish src/Erp.Api/Erp.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app

RUN apk add --no-cache curl tzdata

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
