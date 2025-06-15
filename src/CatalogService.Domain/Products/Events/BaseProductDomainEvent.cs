using System.Diagnostics.CodeAnalysis;

namespace CatalogService.Domain.Products.Events;

[ExcludeFromCodeCoverage]
public abstract record class BaseProductDomainEvent(int ProductId, string Name, decimal Price, int Amount) : IDomainEvent;

