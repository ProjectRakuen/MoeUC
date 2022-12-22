namespace Moe.Core.Caching;

public class CacheKey
{
    public string Key { get; }

    public TimeSpan CacheTime { get; }

    public CacheKey(string key, TimeSpan cacheTime)
    {
        Key = key;
        CacheTime = cacheTime;
    }
}