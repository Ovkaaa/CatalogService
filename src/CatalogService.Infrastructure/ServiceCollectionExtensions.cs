using Azure.Messaging.ServiceBus;
using CatalogService.Application.Interfaces.Events;
using CatalogService.Application.Interfaces.Repositories;
using CatalogService.Domain.Categories;
using CatalogService.Infrastructure.Configurations;
using CatalogService.Infrastructure.Context;
using CatalogService.Infrastructure.Events;
using CatalogService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;

namespace CatalogService.Infrastructure;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isTestingEnv)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        if (!isTestingEnv)
        {
            string? connectionString = configuration.GetConnectionString("CatalogDb");
            services.AddDbContext<CatalogDbContext>(opt =>
                opt.UseSqlServer(connectionString));
        }

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IRepository<Category>, CategoryRepository>();

        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.Configure<EventPublisherOptions>(configuration.GetSection(EventPublisherOptions.SectionName));
        services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();

        return services;
    }
}