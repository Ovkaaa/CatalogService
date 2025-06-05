using CatalogService.Domain.Entities;

namespace CatalogService.Application.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId, CancellationToken cancellationToken);
}
