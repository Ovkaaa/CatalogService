using System.Diagnostics.CodeAnalysis;

namespace CatalogService.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public class AzureServiceBusOptions
{
    public const string SectionName = "AzureServiceBusOptions";
    
    public string ConnectionString { get; set; } = string.Empty;
}
