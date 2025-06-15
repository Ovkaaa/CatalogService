using System.Diagnostics.CodeAnalysis;
using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Application.Interfaces.Services;

namespace CatalogService.Application.Services;

[ExcludeFromCodeCoverage]
public class ServiceBase<TEntity>(IRepository<TEntity> repository) : IEntityService<TEntity> where TEntity : class
{
    public virtual Task AddEntityAsync(TEntity entity, CancellationToken cancellationToken) => repository.AddEntityAsync(entity, cancellationToken);

    public virtual Task DeleteEntityAsync(int id, CancellationToken cancellationToken) => repository.DeleteEntityAsync(id, cancellationToken);

    public virtual Task<List<TEntity>> GetEntitiesAsync(CancellationToken cancellationToken) => repository.GetEntitiesAsync(cancellationToken);

    public virtual Task<TEntity?> GetEntityByIdAsync(int id, CancellationToken cancellationToken) => repository.GetEntityByIdAsync(id, cancellationToken);

    public  virtual Task UpdateEntityAsync(TEntity entity, CancellationToken cancellationToken) => repository.UpdateEntityAsync(entity, cancellationToken);
}
