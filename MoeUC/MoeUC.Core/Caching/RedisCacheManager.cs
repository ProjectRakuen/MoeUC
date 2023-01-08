using StackExchange.Redis;

namespace MoeUC.Core.Caching;

public class RedisCacheManager : ICacheManager
{
    private readonly IDatabase _database;


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