using Scandoc.LocalCaching.Examples.Infrastructure;

namespace Scandoc.LocalCaching.Examples.Caches;

public class SummariesCache : LocalCacheBase<string[]>, ISummariesCache
{
    private readonly IDatabase _database;

    public SummariesCache(IDatabase database)
    {
        _database = database;
    }

    protected override async Task<string[]> FactoryAsync()
    {
        return await _database.GetSummariesAsync(CancellationToken.None);
    }
}