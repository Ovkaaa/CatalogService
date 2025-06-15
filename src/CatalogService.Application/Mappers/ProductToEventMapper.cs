using CatalogService.Application.Interfaces.Mappers;
using CatalogService.Domain;
using CatalogService.Domain.Products;
using CatalogService.Domain.Products.Events;

namespace CatalogService.Application.Mappers;

public class ProductToEventMapper : IEntityToEventMapper<Product>
{
    public TEvent Map<TEvent>(Product entity) where TEvent : class, IDomainEvent
    {
        return typeof(TEvent) switch
        {
            Type t when t == typeof(ProductCreatedDomainEvent) => (TEvent)(object)new ProductCreatedDomainEvent(entity.Id, entity.Name, entity.Price, entity.Amount),
            Type t when t == typeof(ProductUpdatedDomainEvent) => (TEvent)(object)new ProductUpdatedDomainEvent(entity.Id, entity.Name, entity.Price, entity.Amount),
            _ => throw new InvalidOperationException($"Unsupported event type: {typeof(TEvent).Name}")
        };
    }
}
