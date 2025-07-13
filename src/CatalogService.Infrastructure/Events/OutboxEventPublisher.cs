using CatalogService.Application.Interfaces.Events;
using CatalogService.Infrastructure.Context;
using CatalogService.Infrastructure.Outbox;
using System.Text.Json;

namespace CatalogService.Infrastructure.Events;

public class OutboxEventPublisher(CatalogDbContext dbContext) : IEventPublisher
{
    public async Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            EventType = typeof(T).Name,
            ContentType = "application/json",
            Content = JsonSerializer.Serialize(domainEvent),
            OccurredOn = DateTime.UtcNow
        };

        await dbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
    }
}
