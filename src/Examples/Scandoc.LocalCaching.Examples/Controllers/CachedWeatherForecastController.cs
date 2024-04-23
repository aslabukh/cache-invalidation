using Microsoft.AspNetCore.Mvc;
using Scandoc.LocalCaching.Examples.Caches;

namespace Scandoc.LocalCaching.Examples.Controllers;

[ApiController]
[Route("[controller]")]
public class CachedWeatherForecastController : ControllerBase
{
    private readonly ISummariesCache _cache;

    public CachedWeatherForecastController(ISummariesCache cache)
    {
        _cache = cache;
    }

    [HttpGet(Name = "GetWeatherForecastWithCachedSummaries")]
    public IEnumerable<WeatherForecast> Get()
    {
        var summaries = _cache.Get().ToArray();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            })
            .ToArray();
    }
}