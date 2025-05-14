using CatalogService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Tests;

public static class TestDbContextFactory
{
    public static CatalogDbContext Create()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase($"CatalogDb_{Guid.NewGuid()}")
            .Options;

        return new CatalogDbContext(options);
    }
}
