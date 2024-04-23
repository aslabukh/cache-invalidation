namespace Scandoc.LocalCaching.Builder;

public interface ILocalCachingBuilder
{
    /// <summary>
    /// Adds local cache and binds it to reset key.
    /// </summary>
    /// <typeparam name="TService">Local cache service.</typeparam>
    /// <typeparam name="TImplementation">Local cache implementation.</typeparam>
    /// <param name="resetKey">The key, receipt of which will reset this cache.</param>
    /// <returns></returns>
    ILocalCachingBuilder AddCache<TService, TImplementation>(
        CacheResetKey resetKey)
        where TService : class, IResettable
        where TImplementation : class, TService;
}