FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

WORKDIR /
COPY . .

#### Restore and Build stage
RUN dotnet restore "service/src/PackageService.sln"
RUN dotnet build "service/src/PackageService.sln" --no-restore --configuration "Release"

#### Publish stage
FROM build as publish

RUN dotnet publish --no-restore "service\src\PackageService.Api\PackageService.Api.csproj" -c Release -o /app/publish

#### Final stage
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine3.11 AS final
EXPOSE 80
EXPOSE 443
WORKDIR /app
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
COPY --from=publish /app/publish .
ENTRYPOINT dotnet PackageService.Api.dll