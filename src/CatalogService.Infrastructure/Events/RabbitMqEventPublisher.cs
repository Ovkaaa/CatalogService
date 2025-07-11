using CatalogService.Application.Interfaces.Events;
using CatalogService.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CatalogService.Infrastructure.Events;

public class RabbitMqEventPublisher(
    IOptions<RabbitMqOptions> rabbitMqOptions,
    IOptions<EventPublisherOptions> eventPublisherOptions) : IEventPublisher
{
    public async Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken)
    {
        var eventType = typeof(T).Name;

        if (!eventPublisherOptions.Value.EventQueues.TryGetValue(eventType, out var queueName))
        {
            throw new InvalidOperationException($"Queue not configured for event type: {eventType}");
        }

        using var connection = await CreateConnectionAsync(rabbitMqOptions.Value);
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken
        );

        var mesaage = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(domainEvent));

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