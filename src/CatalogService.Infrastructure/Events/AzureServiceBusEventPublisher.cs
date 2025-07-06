using Azure.Messaging.ServiceBus;
using CatalogService.Application.Interfaces.Events;
using CatalogService.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CatalogService.Infrastructure.Events;

public class AzureServiceBusEventPublisher(
    ServiceBusClient client,
    IOptions<EventPublisherOptions> options) : IEventPublisher
{
    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken)
    {
        var eventType = typeof(T).Name;

        if (!options.Value.EventQueues.TryGetValue(eventType, out var queueName))
        {
            throw new InvalidOperationException($"Queue not configured for event type: {eventType}");
        }

        var sender = client.CreateSender(queueName);
        var messageBody = JsonSerializer.Serialize(@event);
        var message = new ServiceBusMessage(messageBody)
        {
            ContentType = "application/json",
            Subject = eventType
        };

        await sender.SendMessageAsync(message, cancellationToken);
    }
}
