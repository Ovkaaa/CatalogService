using System.Diagnostics.CodeAnalysis;
using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Application.Interfaces.Services;
using CatalogService.Domain.Categories;

namespace CatalogService.Application.Services;

[ExcludeFromCodeCoverage]
public class CategoryService(IRepository<Category> repository) : ServiceBase<Category>(repository), IEntityService<Category>
{
}
