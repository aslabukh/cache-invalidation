namespace Scandoc.LocalCaching.Examples.Infrastructure;

public interface IDatabase
{
    Task<string[]> GetSummariesAsync(CancellationToken cancellationToken);
}