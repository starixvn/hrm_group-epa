FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder

WORKDIR /src

# Copy solution and project files
COPY *.sln .
COPY SMS.EPA.Resources/*.csproj SMS.EPA.Resources/
COPY SMS.EPA.Core/*.csproj SMS.EPA.Core/
COPY SMS.EPA.Models/*.csproj SMS.EPA.Models/
COPY SMS.EPA.Criteria/*.csproj SMS.EPA.Criteria/
COPY SMS.EPA.Repositories/*.csproj SMS.EPA.Repositories/
COPY SMS.EPA.Services/*.csproj SMS.EPA.Services/
COPY SMS.EPA.Site/*.csproj SMS.EPA.Site/

# Copy the rest of the source code
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
# Restore dependencies
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore -v n -r linux-x64 SMS.EPA.Site/SMS.EPA.Site.csproj

# Copy source codes
COPY . .

WORKDIR /src/SMS.EPA.Site
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -r linux-x64 --self-contained false -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine AS base

WORKDIR /app
COPY --from=builder /app .

ENV TZ="Asia/Singapore"
ENV COMPlus_EnableDiagnostics=0

# Don't run as root
RUN adduser --disabled-password \
    --home /app \
    --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser

ENTRYPOINT ["dotnet", "SMS.EPA.Site.dll"]