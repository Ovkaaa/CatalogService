# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project files needed for restore - improves caching
COPY CatalogService.EventPublisher/*.csproj CatalogService.EventPublisher/
COPY CatalogService.Domain/*.csproj CatalogService.Domain/
COPY CatalogService.Application/*.csproj CatalogService.Application/
COPY CatalogService.Infrastructure/*.csproj CatalogService.Infrastructure/

RUN dotnet restore CatalogService.EventPublisher/CatalogService.EventPublisher.csproj

# Copy everything else
COPY . .

# Build and publish
RUN dotnet publish ./CatalogService.EventPublisher/CatalogService.EventPublisher.csproj -c Release -o /out

# Stage 2: Runtime
FROM mcr.microsoft.com/azure-functions/dotnet:4-dotnet8
WORKDIR /home/site/wwwroot

# Copy published output from build stage
COPY --from=build /out .

# Azure Functions runtime starts automatically
