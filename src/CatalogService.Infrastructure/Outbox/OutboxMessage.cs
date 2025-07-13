namespace CatalogService.Infrastructure.Outbox;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public string Content { get; set; } = default!;
    public OutboxMessageStatus Status { get; set; } = OutboxMessageStatus.Pending;
    public string ErrorMessage { get; set; } = default!;
    public DateTime OccurredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
}
