using Common;

namespace MazeConsumer.Services;

public class IngestService : IIngestService
{
    private readonly IMazeRestClient _mazeRestClient;
    private readonly ILogger<IngestService> _logger;
    private readonly IIndexingService _indexingService;

    public IngestService(IMazeRestClient mazeRestClient, IIndexingService indexingService, ILogger<IngestService> logger)
    {
        _mazeRestClient = mazeRestClient;
        _logger = logger;
        _indexingService = indexingService;
    }
    public async Task Ingest(ScraperData scraperData)
    {
        _logger.LogInformation("Processing Page - {Page}", scraperData.PageNumber);
        var response = await _mazeRestClient.GetTvShows(scraperData.PageNumber);
        if (response == null)
        {
            _logger.LogError($"Failed to fetch Tv Shows for page {scraperData.PageNumber}.");
            return;
        }
        await GetCast(response);
        await _indexingService.Index(scraperData.PageNumber, response);
        scraperData.RowFetched = response.Count();
    }

    private async Task GetCast(IEnumerable<TvShow> tvShows)
    {
        foreach (var tvShow in tvShows)
        {
            var response = await _mazeRestClient.GetTvShowCast(tvShow.Id);
            if (response == null)
            {
                _logger.LogError($"Failed to get Cast Information for {tvShow.Id} : {tvShow.Name}.");
                continue;
            }
            tvShow.Casts = response;
        }
    }
}
