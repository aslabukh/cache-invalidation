namespace Scandoc.LocalCaching;

/// <inheritdoc cref="ILocalCache{TData}"/>
public abstract class LocalCacheBase<TData> : ILocalCache<TData>
{
    private Lazy<Task<TData>> _cache;

    protected LocalCacheBase()
    {
        _cache = Init();
    }

    #region Async

    public async ValueTask<TData> GetAsync(CancellationToken token)
    {
        return await _cache.Value.WaitAsync(token);
    }

    #endregion

    #region Sync

    public TData Get()
    {
        return _cache.Value.GetAwaiter().GetResult();
    }

    #endregion

    public void Reset()
    {
        _cache = Init();
    }

    protected abstract Task<TData> FactoryAsync();

    private Lazy<Task<TData>> Init()
    {
        return new Lazy<Task<TData>>(FactoryAsync, LazyThreadSafetyMode.ExecutionAndPublication);
    }
}