FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
COPY . .
RUN dotnet build "src/Fiap.Soat.SmartMechanicalWorkshop.Api/Fiap.Soat.SmartMechanicalWorkshop.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Fiap.Soat.SmartMechanicalWorkshop.Api/Fiap.Soat.SmartMechanicalWorkshop.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fiap.Soat.SmartMechanicalWorkshop.Api.dll"]
