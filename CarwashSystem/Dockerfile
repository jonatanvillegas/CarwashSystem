# =========================
# Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Escuchar el puerto dinámico de Coolify
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

EXPOSE 8080

# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["UI.csproj", "./"]
RUN dotnet restore "UI.csproj"

COPY . .
RUN dotnet publish "UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# =========================
# Final
# =========================
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UI.dll"]
