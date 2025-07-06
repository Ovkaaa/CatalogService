using System.Diagnostics.CodeAnalysis;

namespace CatalogService.Domain.Products.Events;

[ExcludeFromCodeCoverage]
public record ProductCreatedDomainEvent(int ProductId, string Name, decimal Price, int Amount) : BaseProductDomainEvent(ProductId, Name, Price, Amount);

