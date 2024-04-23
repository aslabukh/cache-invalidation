using Microsoft.Extensions.DependencyInjection;
using Scandoc.LocalCaching.Builder;
using Scandoc.LocalCaching.Internal;
using StackExchange.Redis;

namespace Scandoc.LocalCaching;

public static class RegistrationExtensions
{
    /// <summary>
    /// Adds local caching and reset functionality.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="systemName">Name of this application.</param>
    /// <param name="redis">Redis connection.</param>
    /// <param name="cachesConfiguration">Registers local caches.</param>
    /// <returns></returns>
    public static IServiceCollection AddLocalCaching(
        this IServiceCollection services,
        string systemName,
        IConnectionMultiplexer redis,
        Action<ILocalCachingBuilder> cachesConfiguration)
    {
        lock (services)
        {
            if (services.FirstOrDefault(d => d.ServiceType == typeof(Internal.System)) != null)
                throw new InvalidOperationException("LocalCaching is already registered");
        }

        services
            .AddSingleton(new Internal.System(systemName))
            .AddSingleton<Caches>()
            .AddHostedService<RedisCacheResetJob>()
            .AddSingleton(new RedisWrapper(redis));

        var builder = new LocalCachingBuilder(services);
        cachesConfiguration(builder);

        return services;
    }
}