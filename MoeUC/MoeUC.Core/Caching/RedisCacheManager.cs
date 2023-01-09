using Microsoft.Extensions.Configuration;
using MoeUC.Core.Infrastructure.Dependency;
using MoeUC.Core.Redis;
using StackExchange.Redis;

namespace MoeUC.Core.Caching;

public class RedisCacheManager : ICacheManager,IScoped
{
    private readonly MoeRedisClient _redisClient;
    private readonly IConfiguration _configuration;

    private readonly IDatabase _database;

    public RedisCacheManager(MoeRedisClient redisClient, IConfiguration configuration)
    {
        _redisClient = redisClient;
        _configuration = configuration;

        _database = _redisClient.GetDatabase();
    }

    public T Get<T>(CacheKey key, Func<T> acquire)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
    {
        throw new NotImplementedException();
    }

    public void Remove(CacheKey key)
    {
        throw new NotImplementedException();
    }

    public void Set(CacheKey key, object item)
    {
        throw new NotImplementedException();
    }

    public bool IsSet(CacheKey key)
    {
        throw new NotImplementedException();
    }
}