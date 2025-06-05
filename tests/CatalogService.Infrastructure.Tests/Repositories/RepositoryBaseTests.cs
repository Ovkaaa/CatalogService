using CatalogService.Infrastructure.Context;
using CatalogService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Tests.Repositories;

public abstract class RepositoryBaseTests<TEntity> where TEntity : class
{
    protected readonly CatalogDbContext _dbContext;
    protected readonly RepositoryBase<TEntity> _repository;

    protected RepositoryBaseTests(Func<CatalogDbContext, RepositoryBase<TEntity>> repositoryFactory)
    {
        _dbContext = TestDbContextFactory.Create();
        _repository = repositoryFactory(_dbContext);
    }

    protected abstract void AssertEntityEquels(TEntity actualEntity, int expectedId);
    protected abstract void AssertUpdatedEntityEquels(TEntity actualEntity, int expectedId);
    protected abstract TEntity CreateEntity(int withId);
    protected abstract TEntity UpdateEntity(TEntity entity);

    [Fact]
    public async Task AddAsync_ShouldAddCategory()
    {
        // Arrange
        int entityId = 1;
        var entity = CreateEntity(entityId);

        // Act
        await _repository.AddEntityAsync(entity, CancellationToken.None);
        var saved = await _dbContext.Set<TEntity>().FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(saved);
        AssertEntityEquels(saved, entityId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategory()
    {
        // Arrange
        int entityId = 1;
        var entity = CreateEntity(entityId);
        _dbContext.Set<TEntity>().Add(entity);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetEntityByIdAsync(entityId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        AssertEntityEquels(result, entityId);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyCategory()
    {
        // Arrange
        int entityId = 1;
        var entity = CreateEntity(entityId);
        _dbContext.Set<TEntity>().Add(entity);
        await _dbContext.SaveChangesAsync();

        // Act
        var updatedEntity = UpdateEntity(entity);
        await _repository.UpdateEntityAsync(updatedEntity, CancellationToken.None);

        // Assert
        var actualEntity = await _dbContext.Set<TEntity>().FindAsync(entityId);
        Assert.NotNull(actualEntity);
        AssertUpdatedEntityEquels(actualEntity, entityId);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCategory()
    {
        // Arrange
        int entityId = 1;
        var entity = CreateEntity(entityId); 
        _dbContext.Set<TEntity>().Add(entity);
        await _dbContext.SaveChangesAsync();

        // Act
        await _repository.DeleteEntityAsync(entityId, CancellationToken.None);

        // Assert
        var exists = await _dbContext.Set<TEntity>().FindAsync(entityId);
        Assert.Null(exists);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        // Arrange
        _dbContext.Set<TEntity>().AddRange(
            CreateEntity(1),
            CreateEntity(2)
        );
        await _dbContext.SaveChangesAsync();

        // Act
        var all = await _repository.GetEntitiesAsync(CancellationToken.None);

        // Assert
        Assert.Equal(2, all.Count);
    }
}
