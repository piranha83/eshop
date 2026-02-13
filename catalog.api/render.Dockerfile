FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY ["catalog.api/Catalog.Api.csproj", "catalog.api/"]
COPY . .
WORKDIR /src/catalog.api
RUN dotnet build -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]