using CatalogService.Domain.Categories;
using CatalogService.Infrastructure.Context;
using CatalogService.Infrastructure.Repositories;

namespace CatalogService.Infrastructure.Tests.Repositories;

public class CategoryRepositoryTests : RepositoryBaseTests<Category>
{
    public CategoryRepositoryTests() : base((CatalogDbContext dbContext) => new CategoryRepository(dbContext))
    {
    }

    protected override void AssertEntityEquels(Category actualEntity, int expectedId)
    {
        ArgumentNullException.ThrowIfNull(actualEntity);
        Assert.Equal(expectedId, actualEntity.Id);
        Assert.Equal($"Test {expectedId}", actualEntity.Name);
    }

    protected override void AssertUpdatedEntityEquels(Category actualEntity, int expectedId)
    {
        ArgumentNullException.ThrowIfNull(actualEntity);
        Assert.Equal(expectedId, actualEntity.Id);
        Assert.Equal($"Updated Test {expectedId}", actualEntity.Name);
    }

    protected override Category CreateEntity(int withId)
    {
        return new Category
        {
            Id = withId,
            Name = $"Test {withId}"
        };
    }

    protected override Category UpdateEntity(Category entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        entity.Name = $"Updated Test {entity.Id}";

        return entity;
    }
}
