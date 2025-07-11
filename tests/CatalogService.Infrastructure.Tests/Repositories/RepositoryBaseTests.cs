using CatalogService.Infrastructure.Context;
using CatalogService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Tests.Repositories;

public abstract class RepositoryBaseTests<TEntity> where TEntity : class
{
    protected CatalogDbContext DbContext { get; init; }
    protected RepositoryBase<TEntity> Repository { get; init; }

    protected RepositoryBaseTests(Func<CatalogDbContext, RepositoryBase<TEntity>> repositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(repositoryFactory);
        DbContext = TestDbContextFactory.Create();
        Repository = repositoryFactory(DbContext);
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
        await Repository.AddEntityAsync(entity, CancellationToken.None);
        var saved = await DbContext.Set<TEntity>().FirstOrDefaultAsync();

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
        DbContext.Set<TEntity>().Add(entity);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await Repository.GetEntityByIdAsync(entityId, CancellationToken.None);

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
        DbContext.Set<TEntity>().Add(entity);
        await DbContext.SaveChangesAsync();

        // Act
        var updatedEntity = UpdateEntity(entity);
        await Repository.UpdateEntityAsync(updatedEntity, CancellationToken.None);

        // Assert
        var actualEntity = await DbContext.Set<TEntity>().FindAsync(entityId);
        Assert.NotNull(actualEntity);
        AssertUpdatedEntityEquels(actualEntity, entityId);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCategory()
    {
        // Arrange
        int entityId = 1;
        var entity = CreateEntity(entityId);
        DbContext.Set<TEntity>().Add(entity);
        await DbContext.SaveChangesAsync();

        // Act
        await Repository.DeleteEntityAsync(entityId, CancellationToken.None);

        // Assert
        var exists = await DbContext.Set<TEntity>().FindAsync(entityId);
        Assert.Null(exists);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        // Arrange
        DbContext.Set<TEntity>().AddRange(
            CreateEntity(1),
            CreateEntity(2)
        );
        await DbContext.SaveChangesAsync();

        // Act
        var all = await Repository.GetEntitiesAsync(CancellationToken.None);

        // Assert
        Assert.Equal(2, all.Count);
    }
}
