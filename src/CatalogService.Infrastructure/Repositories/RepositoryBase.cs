using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Repositories;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected CatalogDbContext DbContext { get; init; }
    protected DbSet<TEntity> DbSet { get; init; }

    protected RepositoryBase(CatalogDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        DbContext = dbContext;
        DbSet = dbContext.Set<TEntity>();
    }

    public async Task AddEntityAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteEntityAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await DbSet.FindAsync([id], cancellationToken);
        if (entity != null) DbSet.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<List<TEntity>> GetEntitiesAsync(CancellationToken cancellationToken) => DbSet.ToListAsync(cancellationToken);

    public async Task<TEntity?> GetEntityByIdAsync(int id, CancellationToken cancellationToken) => await DbSet.FindAsync([id], cancellationToken);

    public Task UpdateEntityAsync(TEntity entity, CancellationToken cancellationToken)
    {
        DbSet.Update(entity);
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}
