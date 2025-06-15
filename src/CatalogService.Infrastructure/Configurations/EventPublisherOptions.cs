using System.Diagnostics.CodeAnalysis;

namespace CatalogService.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public class EventPublisherOptions
{
    public const string SectionName = "EventPublisherOptions";

    public Dictionary<string, string> EventQueues { get; set; } = [];
}
