using CatalogService.Application.Interfaces.Mappers;
using CatalogService.Application.Interfaces.Services;
using CatalogService.Application.Mappers;
using CatalogService.Application.Services;
using CatalogService.Domain.Categories;
using CatalogService.Domain.Products;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace CatalogService.Application;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IEntityToEventMapper<Product>, ProductToEventMapper>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IEntityService<Category>, CategoryService>();

        return services;
    }
}