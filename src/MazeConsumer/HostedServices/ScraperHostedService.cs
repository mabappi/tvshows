using MazeConsumer.Services;

namespace MazeConsumer.HostedServices;

public class ScraperHostedService : IHostedService, IDisposable
{
    private readonly ILogger<ScraperHostedService> _logger;
    private readonly IScraperService _scraperService;
    private Timer? _timer = null;

    public ScraperHostedService(ILogger<ScraperHostedService> logger, IScraperService scraperService)
    {
        _logger = logger;
        _scraperService = scraperService;
    }

    public Task StartAsync(CancellationToken token)
    {
        _logger.LogInformation("Scraper Hosted Service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        _logger.LogInformation("Scraping started.");
        await _scraperService.Scrap();
    }

    public Task StopAsync(CancellationToken token)
    {
        _logger.LogInformation("Scraper Hosted Service running.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}