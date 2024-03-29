﻿using Microsoft.Extensions.Configuration;
using MoeUC.Core.Helpers;
using MoeUC.Core.Infrastructure.Dependency;
using StackExchange.Redis;

namespace MoeUC.Core.Redis;

public class MoeRedisClient : ISingleton
{
    private readonly string _redisConnectionString;
    private volatile ConnectionMultiplexer? _connectionMultiplexer;
    private readonly object _lock = Guid.NewGuid();
    private readonly int _databaseId;

    public MoeRedisClient(IConfiguration configuration)
    {
        var config = configuration.ToString();
        _redisConnectionString = configuration["RedisConnectionString"]!;

        if (string.IsNullOrWhiteSpace(_redisConnectionString))
            throw new ArgumentNullException();

        var dataBaseIdStr = configuration.GetSection("RedisDatabaseId").Value;

        if (string.IsNullOrWhiteSpace(dataBaseIdStr))
            throw new ArgumentNullException();

        _databaseId = int.TryParse(dataBaseIdStr, out var databaseId) ?
            databaseId : 0;
    }

    #region Conncetion

    private ConnectionMultiplexer GetConnection()
    {
        if (_connectionMultiplexer is { IsConnected: true })
        {
            return _connectionMultiplexer;
        }

        lock (_lock)
        {
            if (_connectionMultiplexer is {IsConnected: true})
                return _connectionMultiplexer;

            // Dispose if disconnected
            _connectionMultiplexer?.Dispose();

            _connectionMultiplexer = ConnectionMultiplexer.Connect(_redisConnectionString);
        } 

        return _connectionMultiplexer;
    }

    public IDatabase GetDatabase()
    {
        return GetConnection().GetDatabase(_databaseId);
    }

    #endregion

    #region Pub/Sub

    public long Publish<T>(string channel, T message, MoeSerializeType serializeType = MoeSerializeType.Json)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        var subscriber = GetSubscriber();
                
        if (serializeType == MoeSerializeType.Proto)
                return subscriber.Publish(RedisChannel.Literal(channel), ConvertHelper.ProtoSerialize(message));
    
        return subscriber.Publish(RedisChannel.Literal(channel) ,message is string str ? str : ConvertHelper.JsonSerialize(message));
    }

    private ISubscriber GetSubscriber()
    {
        return GetConnection().GetSubscriber();
    }

    public void Subscribe(string channel, Action<RedisChannel, RedisValue> handelAction)
    {
        GetSubscriber().Subscribe(RedisChannel.Literal(channel), handelAction);
    }

    public async Task SubscribeAsync(string channel, Action<RedisChannel, RedisValue> handelAction)
    {
        await GetSubscriber().SubscribeAsync(RedisChannel.Literal(channel), handelAction);
    }


    #endregion

    #region Redis Lock

    public bool PerformActionWithLock(string lockName, TimeSpan expiresIn, Action action)
    {
        var database = GetDatabase();
        
        if (!string.IsNullOrEmpty(database.StringGet(lockName)))
            return false;

        try
        {
            database.StringSet(lockName, "distributed lock", expiresIn);
            action();
            return true;
        }
        finally
        {
            database.KeyDelete(lockName);
        }
    }

    public async Task<bool> PerformActionWithLockAsync(string lockName, TimeSpan expiresIn, Func<Task> action)
    {
        var database = GetDatabase();

        if (!string.IsNullOrEmpty(database.StringGet(lockName)))
            return false;

        try
        {
            await database.StringSetAsync(lockName, "distributed lock", expiresIn);
            await action();
            return true;
        }
        finally
        {
            await database.KeyDeleteAsync(lockName);
        }
    }



    #endregion
}