using System.Diagnostics.CodeAnalysis;
using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Context;
using CatalogService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Infrastructure;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config, bool isTestingEnv)
    {
        if (!isTestingEnv)
        {
            string? connectionString = config.GetConnectionString("CatalogDb");
            services.AddDbContext<CatalogDbContext>(opt =>
                opt.UseSqlServer(connectionString));
        }

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IRepository<Category>, CategoryRepository>();

        return services;
    }
}