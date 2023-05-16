#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src/Catalog/Catalog.IntegrationEventSenderHost
COPY ["/Catalog.IntegrationEventSenderHost.csproj", "/"]
RUN dotnet restore "/Catalog.IntegrationEventSenderHost.csproj"
COPY . .
WORKDIR "/src/Catalog/Catalog.IntegrationEventSenderHost"
RUN dotnet build "Catalog.IntegrationEventSenderHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.IntegrationEventSenderHost.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.IntegrationEventSenderHost.dll"]