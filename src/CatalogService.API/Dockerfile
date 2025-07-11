# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj(s) and restore dependencies
COPY CatalogService.API/CatalogService.API.csproj ./CatalogService.API/
COPY CatalogService.Application/CatalogService.Application.csproj ./CatalogService.Application/
COPY CatalogService.Domain/CatalogService.Domain.csproj ./CatalogService.Domain/
COPY CatalogService.Infrastructure/CatalogService.Infrastructure.csproj ./CatalogService.Infrastructure/

RUN dotnet restore ./CatalogService.API/CatalogService.API.csproj

# Copy remaining source code
COPY . .

# Build and publish release build to /app/publish
RUN dotnet publish ./CatalogService.API/CatalogService.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Run the app
ENTRYPOINT ["dotnet", "CatalogService.API.dll"]
