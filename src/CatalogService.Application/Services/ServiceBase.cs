using System.Diagnostics.CodeAnalysis;
using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Application.Interfaces.Services;

namespace CatalogService.Application.Services;

[ExcludeFromCodeCoverage]
public class ServiceBase<TEntity>(IRepository<TEntity> repository) : IEntityService<TEntity> where TEntity : class
{
    public virtual async Task AddEntityAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await repository.AddEntityAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteEntityAsync(int id, CancellationToken cancellationToken)
    {
        await repository.DeleteEntityAsync(id, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public virtual Task<List<TEntity>> GetEntitiesAsync(CancellationToken cancellationToken) => repository.GetEntitiesAsync(cancellationToken);

    public virtual Task<TEntity?> GetEntityByIdAsync(int id, CancellationToken cancellationToken) => repository.GetEntityByIdAsync(id, cancellationToken);

    public virtual async Task UpdateEntityAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await repository.UpdateEntityAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
