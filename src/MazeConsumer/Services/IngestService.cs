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
    public async Task Ingest(ScraperData scaperData)
    {
        _logger.LogInformation("Processing Page - {Page}", scaperData.PageNumber);
        var response = await _mazeRestClient.GetTvShows(scaperData.PageNumber);
        if (response == null)
        {
            _logger.LogError($"Failed to fetch Tv Shows for page {scaperData.PageNumber}.");
            return;
        }
        await GetCast(response);
        await _indexingService.Index(scaperData.PageNumber, response);
        scaperData.RowFetched = response.Count();
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
