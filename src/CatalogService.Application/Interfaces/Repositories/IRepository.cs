namespace CatalogService.Application.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task AddEntityAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteEntityAsync(int id, CancellationToken cancellationToken);
    Task<List<TEntity>> GetEntitiesAsync(CancellationToken cancellationToken);
    Task<TEntity?> GetEntityByIdAsync(int id, CancellationToken cancellationToken);
    Task UpdateEntityAsync(TEntity entity, CancellationToken cancellationToken);
}
