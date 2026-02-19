ARG configuration
ARG asp_certificate_password
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get install -y ca-certificates

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration
WORKDIR /src
COPY ["catalog.api/Catalog.Api.csproj", "catalog.api/"]
COPY ["catalog.api/certificates/server-encryption-certificate.pfx", "catalog.api/certificates/server-encryption-certificate.pfx"]
COPY . .
WORKDIR /src/catalog.api
RUN dotnet build -c ${configuration} -o /app/build

FROM build AS publish
ARG configuration
ARG render
RUN  if [ "$render" != "true" ]; then \ 
 dotnet dev-certs https --trust; \
 fi
RUN dotnet publish -c ${configuration} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]