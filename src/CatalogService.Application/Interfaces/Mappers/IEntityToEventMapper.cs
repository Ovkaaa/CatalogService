using CatalogService.Domain;

namespace CatalogService.Application.Interfaces.Mappers;

public interface IEntityToEventMapper<TEntity> where TEntity : Entity
{
    TEvent Map<TEvent>(TEntity entity) where TEvent : class, IDomainEvent;
}
