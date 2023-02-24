using MazeConsumer.Models;

namespace MazeConsumer.Services;

public class ScraperService : IScraperService
{
    private readonly IMazeRestClient _mazeRestClient;
    private readonly ILogger<ScraperService> _logger;
    private readonly IIndexingService _indexingService;

    public ScraperService(IMazeRestClient mazeRestClient, IIndexingService indexingService, ILogger<ScraperService> logger)
    {
        _mazeRestClient = mazeRestClient;
        _logger = logger;
        _indexingService = indexingService;
    }
    public async Task Scrap(ScaperData scaperData)
    {
        var response = await _mazeRestClient.GetTvShows(scaperData.PageNumber);
        if (!response.IsSuccessful)
        {
            _logger.LogWarning($"Failed to fetch Tv Shows for page {scaperData.PageNumber}. {response.ErrorMessage}");
            return;
        }
        await GetCast(response.Data);
        await _indexingService.Index(scaperData.PageNumber, response.Data);
    }

    private async Task GetCast(IEnumerable<TvShow>? tvShows)
    {
        foreach (var tvShow in tvShows)
        {
            var response = await _mazeRestClient.GetTvShowCast(tvShow.Id);
            if (!response.IsSuccessful)
            {
                _logger.LogWarning($"Failed to get Cast Information for {tvShow.Id} : {tvShow.Name}. {response.ErrorMessage}");
                continue;
            }
            tvShow.Casts = response.Data;
        }
    }
}
