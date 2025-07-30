using RabbitMQ.Client;

namespace CatalogService.Infrastructure.Clients.Interfaces;

public interface IConnectionProvider
{
    Task<IConnection> GetConnection();
}