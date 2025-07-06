using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Domain.Categories;
using CatalogService.Infrastructure.Context;

namespace CatalogService.Infrastructure.Repositories;

public class CategoryRepository(CatalogDbContext dbContext) : RepositoryBase<Category>(dbContext), IRepository<Category>
{
}
