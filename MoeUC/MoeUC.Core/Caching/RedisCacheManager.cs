using MoeUC.Core.Helpers;
using MoeUC.Core.Infrastructure.Dependency;
using MoeUC.Core.Redis;
using StackExchange.Redis;

namespace MoeUC.Core.Caching;

public class RedisCacheManager : ICacheManager,IScoped
{

    private readonly IDatabase _database;

    public RedisCacheManager(MoeRedisClient redisClient)
    {
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
        var cachedItem = await _database.StringGetAsync(key.Key);

        return cachedItem.HasValue ? default : ConvertHelper.AutoDeserialize<T>(cachedItem!);

    }

    public void Remove(CacheKey key)
    {
        _database.KeyDelete(key.Key);
    }

    public void Set(CacheKey key, object? item)
    {
        _database.StringSet(key.Key, ConvertHelper.AutoSerialize(item));
    }

    public async Task SetAsync(CacheKey key, object? item)
    {
        await _database.StringSetAsync(key.Key, ConvertHelper.AutoSerialize(item));
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