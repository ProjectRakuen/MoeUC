using Microsoft.Extensions.Caching.Memory;
using MoeUC.Service.ServiceBase.Models;

namespace MoeUC.Service.ServiceBase.Caching;

public class MemoryCacheManager : ICacheManager
{
    private readonly IMemoryCache _memoryCache;
    private readonly WorkContext _workContext;

    public MemoryCacheManager(IMemoryCache memoryCache, WorkContext workContext)
    {
        _memoryCache = memoryCache;
        _workContext = workContext;
    }

    public T? Get<T>(CacheKey key, Func<T?> acquire)
    {
        if (_memoryCache.TryGetValue(key.Key, out T? result))
        {
            _workContext.RequestStatistic.CacheRead++;
            return result;
        }

        result = acquire();
        _memoryCache.Set(key.Key, result, new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = key.CacheTime
        });
        _workContext.RequestStatistic.CacheWrite++;

        return result;
    }

    public async Task<T?> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
    {
        if (_memoryCache.TryGetValue(key.Key, out T? result))
        {
            _workContext.RequestStatistic.CacheRead++;
            return result;
        }

        result = await acquire();

        _memoryCache.Set(key.Key, result, new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = key.CacheTime
        });
        _workContext.RequestStatistic.CacheWrite++;

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
        _workContext.RequestStatistic.CacheWrite++;
    }

    public bool IsSet(CacheKey key)
    {
        return _memoryCache.TryGetValue(key, out _);
    }
}