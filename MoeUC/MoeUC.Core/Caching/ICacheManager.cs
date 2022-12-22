namespace MoeUC.Core.Caching;

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
    Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire);

    /// <summary>
    /// remove an cached item by key
    /// </summary>
    /// <param name="key"></param>
    void Remove(CacheKey key);

    void Set(CacheKey key, object item);

    bool IsSet(CacheKey key);
}