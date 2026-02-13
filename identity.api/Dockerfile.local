ARG configuration
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get install -y ca-certificates

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG configuration
WORKDIR /src
COPY ["identity.api/Identity.Api.csproj", "identity.api/"]
COPY . .
WORKDIR /src/identity.api
RUN dotnet build -c ${configuration} -o /app/build

FROM build AS publish
ARG configuration
RUN dotnet dev-certs https --trust
RUN dotnet publish -c ${configuration} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]
