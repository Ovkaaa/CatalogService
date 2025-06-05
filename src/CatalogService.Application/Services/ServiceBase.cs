using System.Diagnostics.CodeAnalysis;
using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Application.Interfaces.Services;

namespace CatalogService.Application.Services;

[ExcludeFromCodeCoverage]
public class ServiceBase<TEntity>(IRepository<TEntity> repository) : IEntityService<TEntity> where TEntity : class
{
    public Task AddEntityAsync(TEntity entity, CancellationToken cancellationToken) => repository.AddEntityAsync(entity, cancellationToken);

    public Task DeleteEntityAsync(int id, CancellationToken cancellationToken) => repository.DeleteEntityAsync(id, cancellationToken);

    public Task<List<TEntity>> GetEntitiesAsync(CancellationToken cancellationToken) => repository.GetEntitiesAsync(cancellationToken);

    public Task<TEntity?> GetEntityByIdAsync(int id, CancellationToken cancellationToken) => repository.GetEntityByIdAsync(id, cancellationToken);

    public Task UpdateEntityAsync(TEntity entity, CancellationToken cancellationToken) => repository.UpdateEntityAsync(entity, cancellationToken);
}
