using Microsoft.Extensions.Configuration;
using MoeUC.Core.Infrastructure.Dependency;
using StackExchange.Redis;

namespace MoeUC.Core.Redis;

public class MoeRedisClient : IScoped
{
    private readonly IConfiguration _configuration;
    private readonly string _redisConnectionString;
    private volatile ConnectionMultiplexer? _connectionMultiplexer;
    private readonly object _lock = Guid.NewGuid();

    public MoeRedisClient(IConfiguration configuration)
    {
        this._configuration = configuration;
        _redisConnectionString = configuration["Redis:ConnectionString"]!;
    }

    private ConnectionMultiplexer GetConnection()
    {
        if (_connectionMultiplexer is { IsConnected: true })
        {
            return _connectionMultiplexer;
        }

        lock (_lock)
        {
            if (_connectionMultiplexer is {IsConnected: true})
                return _connectionMultiplexer;

            // Dispose if disconnected
            _connectionMultiplexer?.Dispose();

            _connectionMultiplexer = ConnectionMultiplexer.Connect(_redisConnectionString);
        } 

        return _connectionMultiplexer;
    }
}