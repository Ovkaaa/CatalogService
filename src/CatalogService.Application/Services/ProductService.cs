using CatalogService.Application.Interfaces.Events;
using CatalogService.Application.Interfaces.Mappers;
using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Application.Interfaces.Services;
using CatalogService.Domain.Products;
using CatalogService.Domain.Products.Events;
using System.Diagnostics.CodeAnalysis;

namespace CatalogService.Application.Services;

[ExcludeFromCodeCoverage]
public class ProductService(
    IProductRepository repository,
    IEntityToEventMapper<Product> entityToEventMapper,
    IEventPublisher eventPublisher) : ServiceBase<Product>(repository), IProductService
{
    public override async Task AddEntityAsync(Product entity, CancellationToken cancellationToken)
    {
        await base.AddEntityAsync(entity, cancellationToken);

        ProductCreatedDomainEvent @event = entityToEventMapper.Map<ProductCreatedDomainEvent>(entity);
        await eventPublisher.PublishAsync(@event, cancellationToken);
    }

    public override async Task UpdateEntityAsync(Product entity, CancellationToken cancellationToken)
    {
        await base.UpdateEntityAsync(entity, cancellationToken);

        ProductUpdatedDomainEvent @event = entityToEventMapper.Map<ProductUpdatedDomainEvent>(entity);
        await eventPublisher.PublishAsync(@event, cancellationToken);
    }

    public Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId, CancellationToken cancellationToken) => repository.GetProductsByCategoryIdAsync(categoryId, cancellationToken);
}
