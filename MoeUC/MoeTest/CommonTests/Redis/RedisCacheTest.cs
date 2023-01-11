using MoeUC.Core.Caching;
using MoeUC.Test.CommonTests.Mocks;

namespace MoeUC.Test.CommonTests.Redis;

public class RedisCacheTest : IClassFixture<RedisCacheManager>
{
    private readonly RedisCacheManager _cacheManager;

    public RedisCacheTest(RedisCacheManager cacheManager)
    {
        _cacheManager = cacheManager;
    }

    [Fact]
    public void TestGetSet()
    {
        var entity = GetMockProtoEntity();
        var key = new CacheKey("MoeTest:RedisCacheTest", TimeSpan.FromMinutes(10), entity.Id);
        var inCacheEntity = _cacheManager.Get(key, () => entity);
        Assert.True(inCacheEntity.Equals(entity));
        Assert.True(_cacheManager.IsSet(key));
        
        _cacheManager.Remove(key);
        Assert.False(_cacheManager.IsSet(key));
    }

    [Fact]
    public async Task TestKeyExpire()
    {
        var entity = GetMockProtoEntity();
        var key = new CacheKey("MoeTest:RedisCacheExpireTest", TimeSpan.FromSeconds(5));
        _cacheManager.Set(key, entity);
        Assert.True(_cacheManager.IsSet(key));

        await Task.Delay(10);
        Assert.False(_cacheManager.IsSet(key));
    }

    private MockProtoEntity GetMockProtoEntity()
    {
        return new MockProtoEntity()
        {
            Name = "Test",
            Num = 10,
            Id = 10,
        };
    }
}