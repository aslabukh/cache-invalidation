namespace Scandoc.LocalCaching.Internal;

internal class KeyedCaches
{
    public KeyedCaches(string key, IEnumerable<IResettable> caches)
    {
        Key = key;
        Caches = caches;
    }

    public string                   Key    { get; }
    public IEnumerable<IResettable> Caches { get; }
}