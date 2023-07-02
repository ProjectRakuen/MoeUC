using MoeUC.Core.Helpers;
using MoeUC.Core.Infrastructure.Dependency;
using MoeUC.Core.Redis;
using MoeUC.Service.ServiceBase.Models;
using StackExchange.Redis;

namespace MoeUC.Service.ServiceBase.Caching;

public class RedisCacheManager : ICacheManager,IScoped
{

    private readonly IDatabase _database;
    private readonly WorkContext _workContext;

    public RedisCacheManager(MoeRedisClient redisClient, WorkContext workContext)
    {
        _workContext = workContext;
        _database = redisClient.GetDatabase();
    }

    public T? Get<T>(CacheKey key, Func<T?> acquire)
    {
        if (IsSet(key))
            return Get<T>(key);

        var toCacheItem = acquire();
        Set(key, toCacheItem);
        return toCacheItem;
    }

    public T? Get<T>(CacheKey key)
    {
        _workContext.RequestStatistic.CacheRead++;
        var cachedItem = _database.StringGet(key.Key);
        return cachedItem.HasValue ? default : ConvertHelper.AutoDeserialize<T>(cachedItem!);
    }

    public async Task<T?> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
    {
        if (await IsSetAsync(key))
        {
            return await GetAsync<T>(key);
        }

        var toCacheItem = await acquire();
        await SetAsync(key, toCacheItem);

        return toCacheItem;
    }

    public async Task<T?> GetAsync<T>(CacheKey key)
    {
        _workContext.RequestStatistic.CacheRead++;
        var cachedItem = await _database.StringGetAsync(key.Key);

        return cachedItem.HasValue ? default : ConvertHelper.AutoDeserialize<T>(cachedItem!);

    }

    public void Remove(CacheKey key)
    {
        _workContext.RequestStatistic.CacheWrite++;
        _database.KeyDelete(key.Key);
    }

    public void Set(CacheKey key, object? item)
    {
        _workContext.RequestStatistic.CacheWrite++;
        _database.StringSet(key.Key, ConvertHelper.AutoSerialize(item), expiry:key.CacheTime);
    }

    public async Task SetAsync(CacheKey key, object? item)
    {
        _workContext.RequestStatistic.CacheWrite++;
        await _database.StringSetAsync(key.Key, ConvertHelper.AutoSerialize(item), expiry:key.CacheTime);
    }

    public bool IsSet(CacheKey key)
    {
        return _database.KeyExists(key.Key);
    }

    public async Task<bool> IsSetAsync(CacheKey key)
    {
        return await _database.KeyExistsAsync(key.Key);
    }
}