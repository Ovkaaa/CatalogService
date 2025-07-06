using CatalogService.Domain.Products;
using CatalogService.Infrastructure.Context;
using CatalogService.Infrastructure.Repositories;

namespace CatalogService.Infrastructure.Tests.Repositories;

public class ProductRepositoryTests : RepositoryBaseTests<Product>
{
    public ProductRepositoryTests() : base((CatalogDbContext dbContext) => new ProductRepository(dbContext))
    {
    }

    protected override void AssertEntityEquels(Product actualEntity, int expectedId)
    {
        ArgumentNullException.ThrowIfNull(actualEntity);
        Assert.Equal(expectedId, actualEntity.Id);
        Assert.Equal($"Test {expectedId}", actualEntity.Name);
    }

    protected override void AssertUpdatedEntityEquels(Product actualEntity, int expectedId)
    {
        ArgumentNullException.ThrowIfNull(actualEntity);
        Assert.Equal(expectedId, actualEntity.Id);
        Assert.Equal($"Updated Test {expectedId}", actualEntity.Name);
    }

    protected override Product CreateEntity(int withId)
    {
        return new Product
        {
            Id = withId,
            Name = $"Test {withId}"
        };
    }

    protected override Product UpdateEntity(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        entity.Name = $"Updated Test {entity.Id}";

        return entity;
    }
}
