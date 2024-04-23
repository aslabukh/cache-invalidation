namespace Scandoc.LocalCaching;

public interface IResettable
{
    /// <summary>
    /// Resets cache. Next "Get" will reload cache. <br/>
    /// <b>Avoid using this method if possible.
    /// Register cache with scoped lifetime or using AddLocalCaching extension instead.</b>
    /// </summary>
    void Reset();
}