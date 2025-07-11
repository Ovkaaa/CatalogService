using System.Diagnostics.CodeAnalysis;

namespace CatalogService.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public class RabbitMqOptions
{
    public const string SectionName = "RabbitMqOptions";

    public string Hostname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
