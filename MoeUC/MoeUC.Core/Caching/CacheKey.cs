using System.Text;

namespace MoeUC.Core.Caching;

public class CacheKey
{
    public string Key { get; }

    public TimeSpan CacheTime { get; }

    public CacheKey(string key, TimeSpan cacheTime)
    {
        Key = key;
        CacheTime = cacheTime;
    }

    public CacheKey(string key, TimeSpan cacheTime, params object[] suffix)
    {
        var keyBuilder = new StringBuilder(key);

        foreach (var item in suffix)
        {
            keyBuilder.Append(':');
            keyBuilder.Append(item);
        }

        Key = keyBuilder.ToString();
        CacheTime = cacheTime;
    }

    public CacheKey WithSuffix(params object[] suffix)
    {
        var keyBuilder = new StringBuilder(Key);

        foreach (var item in suffix)
        {
            keyBuilder.Append(':');
            keyBuilder.Append(item);
        }

        return new CacheKey(Key, CacheTime);
    }

    public long AbsoluteExpireTimeStamp => DateTimeOffset.Now.Add(CacheTime).ToUnixTimeMilliseconds();
}