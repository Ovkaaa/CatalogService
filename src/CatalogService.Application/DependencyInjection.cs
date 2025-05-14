using System.Diagnostics.CodeAnalysis;
using CatalogService.Application.Interfaces.Services;
using CatalogService.Application.Services;
using CatalogService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Application;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IEntityService<Category>, CategoryService>();

        return services;
    }
}