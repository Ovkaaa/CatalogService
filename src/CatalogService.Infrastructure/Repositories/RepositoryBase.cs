using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Repositories;

public abstract class RepositoryBase<TEntity>(CatalogDbContext dbContext) : IRepository<TEntity> where TEntity : class
{
    protected readonly CatalogDbContext _dbContext = dbContext;
    protected readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task AddEntityAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteEntityAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _dbSet.FindAsync([id], cancellationToken);
        if (entity != null) _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<List<TEntity>> GetEntitiesAsync(CancellationToken cancellationToken) => _dbSet.ToListAsync(cancellationToken);

    public async Task<TEntity?> GetEntityByIdAsync(int id, CancellationToken cancellationToken) => await _dbSet.FindAsync([id], cancellationToken);

    public Task UpdateEntityAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
