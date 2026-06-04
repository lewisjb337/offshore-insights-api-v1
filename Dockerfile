# ─── Runtime base ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
# Railway injects PORT; fall back to 8080 for any other host
ENV ASPNETCORE_HTTP_PORTS=8080

# ─── Build ────────────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first so layer cache is reused when only source changes
COPY OffshoreInsights.API/OffshoreInsights.API.csproj                         OffshoreInsights.API/
COPY OffshoreInsights.Application/OffshoreInsights.Application.csproj         OffshoreInsights.Application/
COPY OffshoreInsights.Domain/OffshoreInsights.Domain.csproj                   OffshoreInsights.Domain/
COPY OffshoreInsights.Infrastructure/OffshoreInsights.Infrastructure.csproj   OffshoreInsights.Infrastructure/
COPY OffshoreInsights.Persistence/OffshoreInsights.Persistence.csproj         OffshoreInsights.Persistence/

RUN dotnet restore OffshoreInsights.API/OffshoreInsights.API.csproj

# Copy everything else and build
COPY . .
WORKDIR /src/OffshoreInsights.API
RUN dotnet build OffshoreInsights.API.csproj -c Release -o /app/build

# ─── Publish ──────────────────────────────────────────────────────────────────
FROM build AS publish
RUN dotnet publish OffshoreInsights.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ─── Final image ──────────────────────────────────────────────────────────────
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OffshoreInsights.API.dll"]
