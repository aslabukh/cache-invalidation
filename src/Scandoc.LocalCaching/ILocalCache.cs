namespace Scandoc.LocalCaching;

/// <summary>
/// Represents <typeparamref name="TData" />, cached locally.
/// </summary>
/// <typeparam name="TData">Cached data.</typeparam>
public interface ILocalCache<TData> : IResettable
{
    /// <summary>
    /// Get data or load it, if not cached.
    /// </summary>
    /// <returns>Cached <typeparamref name="TData" />.</returns>
    ValueTask<TData> GetAsync(CancellationToken token);

    /// <summary>
    /// Get data or synchronously load it, if not cached.
    /// </summary>
    /// <returns>Cached <typeparamref name="TData" />.</returns>
    TData Get();
}