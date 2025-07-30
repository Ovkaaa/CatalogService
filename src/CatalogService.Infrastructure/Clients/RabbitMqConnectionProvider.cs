using CatalogService.Infrastructure.Clients.Interfaces;
using CatalogService.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CatalogService.Infrastructure.Clients;

public class RabbitMqConnectionProvider(IOptions<RabbitMqOptions> rabbitMqOptions) : IConnectionProvider, IDisposable
{
    private readonly static SemaphoreSlim _semaphoreSlim = new(1, 1);
    
    private readonly IOptions<RabbitMqOptions> _rabbitMqOptions = rabbitMqOptions;

    private IConnection? _connection;

    public async Task<IConnection> GetConnection()
    {
        await _semaphoreSlim.WaitAsync();

        try
        {
            _connection ??= await CreateConnection();
            return _connection;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _semaphoreSlim.Dispose();
        }
    }

    private Task<IConnection> CreateConnection()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMqOptions.Value.Hostname,
            UserName = _rabbitMqOptions.Value.Username,
            Password = _rabbitMqOptions.Value.Password
        };

        return factory.CreateConnectionAsync();
    }
}
