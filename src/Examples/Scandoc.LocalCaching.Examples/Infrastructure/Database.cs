namespace Scandoc.LocalCaching.Examples.Infrastructure;

public class Database : IDatabase
{
    public Task<string[]> GetSummariesAsync(CancellationToken cancellationToken)
    {
        return File.ReadAllLinesAsync("db.txt", cancellationToken);
    }
}