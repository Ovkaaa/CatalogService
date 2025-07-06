namespace CatalogService.Application.Interfaces.Events;

public interface IEventPublisher
{
    Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken);
}
