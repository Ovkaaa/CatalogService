using System.Diagnostics.CodeAnalysis;
using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Application.Interfaces.Services;
using CatalogService.Domain.Entities;

namespace CatalogService.Application.Services;

[ExcludeFromCodeCoverage]
public class ProductService(IProductRepository repository) : ServiceBase<Product>(repository), IProductService
{
    public Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId, CancellationToken cancellationToken) => repository.GetProductsByCategoryIdAsync(categoryId, cancellationToken);
}
