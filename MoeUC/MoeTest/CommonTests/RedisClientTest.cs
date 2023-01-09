using System.Text;
using MoeUC.Core.Redis;

namespace MoeUC.Test.CommonTests;

public class RedisClientTest : IClassFixture<MoeRedisClient>
{
    private readonly MoeRedisClient _client;

    public RedisClientTest(MoeRedisClient client)
    {
        _client = client;
    }

    [Fact]
    public async Task TestStringGetSet()
    {
        var setValue = Encoding.UTF8.GetBytes("TestValue");
        _client.GetDatabase().StringSet("TestKey", setValue);
        var bytes = _client.GetDatabase().StringGet("TestKey").Box() as byte[];

        Assert.NotNull(bytes);
        var actualValue = Encoding.UTF8.GetString(bytes);

        try
        {
            Assert.Equal("TestValue", actualValue);
        }
        finally
        {
            await _client.GetDatabase().KeyDeleteAsync("TestKey");
        }
    }
}