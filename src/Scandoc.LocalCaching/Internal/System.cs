namespace Scandoc.LocalCaching.Internal;

internal class System
{
    public System(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        Name = name;
        Key = $"{Keys.ListenersKey}:{name}";
    }

    public string Name { get; }

    public string Key { get; }
}