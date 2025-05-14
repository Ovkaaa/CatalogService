using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Context;

namespace CatalogService.Infrastructure.Repositories;

public class CategoryRepository(CatalogDbContext dbContext) : RepositoryBase<Category>(dbContext), IRepository<Category>
{
}
