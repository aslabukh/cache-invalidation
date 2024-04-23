using StackExchange.Redis;

namespace Scandoc.LocalCaching.Internal;

internal class RedisWrapper
{
    public RedisWrapper(IConnectionMultiplexer redis)
    {
        Redis = redis;
    }

    public IConnectionMultiplexer Redis { get; }
}