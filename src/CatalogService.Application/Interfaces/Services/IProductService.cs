using CatalogService.Domain.Entities;

namespace CatalogService.Application.Interfaces.Services;

public interface IProductService : IEntityService<Product>
{
    Task<List<Product>> GetAllCategoryProductsAsync(int categoryId);
}
