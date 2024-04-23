using System.Collections;

namespace Scandoc.LocalCaching.Internal;

internal class Caches : ILookup<string, IResettable>
{
    private readonly ILookup<string, IResettable> _lookupImplementation;

    public Caches(IEnumerable<KeyedCaches> keyedCaches)
    {
        _lookupImplementation = keyedCaches
            .SelectMany(keyed => keyed.Caches
                .Select(cache => (keyed.Key, Cache: cache)))
            .ToLookup(t => t.Key, t => t.Cache);
    }

    #region ILookup implementation

    public IEnumerator<IGrouping<string, IResettable>> GetEnumerator()
    {
        return _lookupImplementation.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_lookupImplementation).GetEnumerator();
    }

    public bool Contains(string key)
    {
        return _lookupImplementation.Contains(key);
    }

    public int Count => _lookupImplementation.Count;

    public IEnumerable<IResettable> this[string key] => _lookupImplementation[key];

    #endregion
}