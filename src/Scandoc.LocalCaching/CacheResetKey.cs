namespace Scandoc.LocalCaching;

public readonly struct CacheResetKey
{
    public CacheResetKey(string key)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        _key = key;
    }

    private readonly string _key;

    public string Key
    {
        get
        {
            if (_key == null)
                throw new InvalidOperationException(
                    "CacheResetKey was created using default constructor which is not allowed");

            return _key;
        }
    }

    public static implicit operator CacheResetKey(string key)
    {
        return new CacheResetKey(key);
    }

    public static implicit operator string(CacheResetKey key)
    {
        return key.Key;
    }

    public static CacheResetKey Everything = "Everything";
}