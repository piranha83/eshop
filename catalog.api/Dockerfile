ARG configuration
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get install -y ca-certificates

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

RUN mkdir -p /aspnet/https
RUN dotnet dev-certs https -v -ep /aspnet/https/aspnetapp.pfx -p password

ARG configuration
WORKDIR /src
COPY ["catalog.api/Catalog.Api.csproj", "catalog.api/"]
COPY . .
WORKDIR /src/catalog.api
RUN dotnet build -c ${configuration} -o /app/build

FROM build AS publish
ARG configuration
RUN dotnet publish -c ${configuration} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]