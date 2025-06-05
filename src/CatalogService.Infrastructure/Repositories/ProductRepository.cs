using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Repositories;

public class ProductRepository(CatalogDbContext dbContext) : RepositoryBase<Product>(dbContext), IProductRepository
{
    public Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId, CancellationToken cancellationToken)
    {
        return _dbSet.Where(p => p.CategoryId == categoryId).ToListAsync(cancellationToken);
    }
}
