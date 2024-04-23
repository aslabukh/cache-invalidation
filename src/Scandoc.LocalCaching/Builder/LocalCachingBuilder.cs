using Microsoft.Extensions.DependencyInjection;
using Scandoc.LocalCaching.Internal;

namespace Scandoc.LocalCaching.Builder;

internal class LocalCachingBuilder : ILocalCachingBuilder
{
    private readonly IServiceCollection _services;

    public LocalCachingBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public ILocalCachingBuilder AddCache<TService, TImplementation>(
        CacheResetKey resetKey)
        where TService : class, IResettable
        where TImplementation : class, TService
    {
        _services.AddSingleton(sp => new KeyedCaches(resetKey, sp.GetServices<TService>()));
        
        _services.AddSingleton<TService, TImplementation>();

        return this;
    }
}