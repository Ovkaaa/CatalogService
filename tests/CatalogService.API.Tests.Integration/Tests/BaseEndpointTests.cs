
using CatalogService.API.Tests.Integration.Factories;
using CatalogService.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.API.Tests.Integration.Tests;

public class BaseEndpointTests(CustomWebApplicationFactory factory) : IAsyncLifetime
{
    private readonly CatalogDbContext _dbContext = factory.Services.GetRequiredService<CatalogDbContext>();

    public async Task InitializeAsync()
    {
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
    }
}
