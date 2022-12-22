namespace Moe.Core.Caching;

public interface ICacheManager
{
    /// <summary>
    /// Get or cache an item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="acquire">Function to get the item</param>
    /// <returns></returns>
    T Get<T>(CacheKey key, Func<T> acquire);

    /// <summary>
    /// Get or cache an item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="acquire">Function to get the item</param>
    /// <returns></returns>
    T GetAsync<T>(CacheKey key, Func<Task<T>> acquire);

    void Remove(CacheKey key);

    void Set(CacheKey key, object item);

    bool IsSet(CacheKey key);

    void Clear();
}