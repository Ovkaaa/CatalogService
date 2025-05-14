using System.Diagnostics.CodeAnalysis;
using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Application.Interfaces.Services;

namespace CatalogService.Application.Services;

[ExcludeFromCodeCoverage]
public class ServiceBase<TEntity>(IRepository<TEntity> repository) : IEntityService<TEntity> where TEntity : class
{
    public Task AddAsync(TEntity entity) => repository.AddAsync(entity);

    public Task DeleteAsync(int id) => repository.DeleteAsync(id);

    public Task<List<TEntity>> GetAllAsync() => repository.GetAllAsync();

    public Task<TEntity?> GetByIdAsync(int id) => repository.GetByIdAsync(id);

    public Task UpdateAsync(TEntity entity) => repository.UpdateAsync(entity);
}
