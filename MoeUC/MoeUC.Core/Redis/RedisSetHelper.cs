
using MoeUC.Core.Caching;
using MoeUC.Core.Helpers;
using MoeUC.Core.Infrastructure.Dependency;
using StackExchange.Redis;

namespace MoeUC.Core.Redis;

public class RedisSetHelper : IScoped
{
    private readonly IDatabase _database;

    public RedisSetHelper(MoeRedisClient redisClient)
    {
        _database = redisClient.GetDatabase();
    }

    public void SetAdd<T>(CacheKey key, T? value)
    {
        var serializedBytes = ConvertHelper.AutoSerialize(value);
        _database.SortedSetAdd(key.Key, serializedBytes, key.AbsoluteExpireTimeStamp);
    }

    public async Task SetAddAsync<T>(CacheKey key, T? value)
    {
        var bytes = ConvertHelper.AutoSerialize(value);
        await _database.SortedSetAddAsync(key.Key, bytes, key.AbsoluteExpireTimeStamp);
    }

    public bool Contains<T>(CacheKey key, T? value)
    {
        var bytes = ConvertHelper.AutoSerialize(value);
        var score = _database.SortedSetScore(key.Key, bytes);

        TrimSet(key.Key);
        if (score == null)
            return false;

        return score > key.AbsoluteExpireTimeStamp;
    }

    private void TrimSet(string key)
    {
        var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        _database.SortedSetRemoveRangeByScore(key, 0, now);
    }
}