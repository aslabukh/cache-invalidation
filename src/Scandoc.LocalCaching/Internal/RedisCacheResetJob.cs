using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Scandoc.LocalCaching.Internal;

internal class RedisCacheResetJob : BackgroundService
{
    private static readonly TimeSpan Delay = TimeSpan.FromMinutes(10);

    private readonly ILogger<RedisCacheResetJob> _logger;
    private readonly Caches                      _caches;
    private readonly IConnectionMultiplexer      _redis;
    
    public RedisCacheResetJob(
        RedisWrapper redisWrapper,
        ILogger<RedisCacheResetJob> logger,
        Caches caches)
    {
        _redis = redisWrapper.Redis;
        _logger = logger;
        _caches = caches;
    }

    private void Reset(string? key)
    {
        if (key == null)
        {
            _logger.LogWarning("Cache key is {CacheResetKey}", "null");
        }
        else if (key == CacheResetKey.Everything)
        {
            var resetCount = 0;
            foreach (var cacheType in _caches.SelectMany(x => x).Distinct())
            {
                cacheType.Reset();
                resetCount++;
            }
            _logger.LogInformation("Reset all {ResetCount} caches by key {CacheResetKey}", resetCount, key);
        }
        else if (!_caches.Contains(key))
        {
            _logger.LogWarning("No cache registered by key {CacheResetKey}", key);
        }
        else
        {
            var resetCount = 0;
            foreach (var cache in _caches[key])
            {
                cache.Reset();
                resetCount++;
            }
            _logger.LogInformation("Reset {ResetCount} caches by key {CacheResetKey}", resetCount, key);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            try
            {
                await _redis.GetSubscriber()
                    .SubscribeAsync(Keys.ResetChannelKey, (_, value) =>
                    {
                        Reset(value);
                    });

                break;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to subscribe on reset channel");
            }

            await Task.Delay(Delay, stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _redis.GetSubscriber().UnsubscribeAllAsync();
        
        await base.StopAsync(cancellationToken);
    }
}