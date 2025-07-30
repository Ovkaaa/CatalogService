using CatalogService.Infrastructure.Clients.Interfaces;
using CatalogService.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Runtime.InteropServices;
using System.Text;

namespace CatalogService.Infrastructure.Clients;

public class RabbitMqClient(
    IConnectionProvider connectionProvider,
    IOptions<EventPublisherOptions> eventPublisherOptions) : IMessageBrokerClient
{
    public async Task PublishAsync(string message, string eventType, CancellationToken cancellationToken)
    {
        if (!eventPublisherOptions.Value.EventQueues.TryGetValue(eventType, out var queueName))
        {
            throw new InvalidOperationException($"Queue not configured for event type: {eventType}");
        }

        using var connection = await connectionProvider.GetConnection();
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken
        );

        var mesaage = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: mesaage,
            cancellationToken: cancellationToken
        );
    }

    private static Task<IConnection> CreateConnectionAsync(RabbitMqOptions options)
    {
        var factory = new ConnectionFactory()
        {
            HostName = options.Hostname,
            UserName = options.Username,
            Password = options.Password
        };
        
        return factory.CreateConnectionAsync();
    }
}
