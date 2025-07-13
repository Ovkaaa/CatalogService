using CatalogService.Infrastructure.Clients.Interfaces;
using CatalogService.Infrastructure.Context;
using CatalogService.Infrastructure.Outbox;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CatalogService.EventPublisher;

public class OutboxProcessorFunction(
    CatalogDbContext dbContext,
    IMessageBrokerClient messageBrokerClient,
    IOptions<OutboxProcessorOptions> options,
    ILogger<OutboxProcessorFunction> logger)
{
    [Function(nameof(OutboxProcessorFunction))]
    public async Task Run([TimerTrigger("%OutboxProcessorSchedule%")] TimerInfo myTimer, CancellationToken cancellationToken)
    {
        var unsent = await dbContext.OutboxMessages
            .Where(x => x.ProcessedOn == null && x.Status == OutboxMessageStatus.Pending)
            .OrderBy(x => x.OccurredOn)
            .Take(options.Value.BatchSize)
            .ToListAsync(cancellationToken);

        foreach (var message in unsent)
        {
            try
            {
                await messageBrokerClient.PublishAsync(message.Content, message.EventType, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to publish message {MessageId} of type {EventType}", message.Id, message.EventType);
                message.Status = OutboxMessageStatus.Failed;
                message.ErrorMessage = ex.Message;
                continue;
            }

            message.ProcessedOn = DateTime.UtcNow;
            message.Status = OutboxMessageStatus.Processed;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Processed {Count} outbox messages at: {DateTime}", unsent.Count, DateTime.Now);
    }
}
