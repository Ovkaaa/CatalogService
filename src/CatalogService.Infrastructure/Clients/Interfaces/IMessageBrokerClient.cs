namespace CatalogService.Infrastructure.Clients.Interfaces;

public interface IMessageBrokerClient
{
    public Task PublishAsync(string message, string eventType, CancellationToken cancellationToken);
}