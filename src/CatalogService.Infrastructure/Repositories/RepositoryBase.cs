using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Repositories;

public abstract class RepositoryBase<TEntity>(CatalogDbContext dbContext) : IRepository<TEntity> where TEntity : class
{
    protected readonly CatalogDbContext _dbContext = dbContext;
    protected readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public Task<List<TEntity>> GetAllAsync() => _dbSet.ToListAsync();

    public async Task<TEntity?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public Task AddAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        return _dbContext.SaveChangesAsync();
    }

    public Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null) _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}
