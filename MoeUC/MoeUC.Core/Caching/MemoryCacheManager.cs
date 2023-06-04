using Microsoft.Extensions.Caching.Memory;
using MoeUC.Core.Caching;

namespace MoeUC.Core.Caching;

public class MemoryCacheManager : ICacheManager
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheManager(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public T? Get<T>(CacheKey key, Func<T?> acquire)
    {
        if (_memoryCache.TryGetValue(key.Key, out T? result) && result != null)
            return result;

        result = acquire();
        _memoryCache.Set(key.Key, result, new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = key.CacheTime
        });

        return result;
    }

    public async Task<T?> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
    {
        if (_memoryCache.TryGetValue(key.Key, out T? result) && result != null)
            return result;
        result = await acquire();

        _memoryCache.Set(key.Key, result, new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = key.CacheTime
        });

        return result;
    }

    public void Remove(CacheKey key)
    {
        _memoryCache.Remove(key.Key);
    }

    public void Set(CacheKey key, object item)
    {
        _memoryCache.Set(key.Key, new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = key.CacheTime,
        });
    }

    public bool IsSet(CacheKey key)
    {
        return _memoryCache.TryGetValue(key, out var value);
    }
}