using CatalogService.Domain.Products;

namespace CatalogService.Application.Interfaces.Services;

public interface IProductService : IEntityService<Product>
{
    Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId, CancellationToken cancellationToken);
}
