using Framework;

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
        if (response?.Data == null || !response.IsSuccessful)
        {
            _logger.LogError(response.ErrorException, $"Failed to fetch Tv Shows for page {scaperData.PageNumber}. {response.ErrorMessage}");
            return;
        }
        await GetCast(response.Data);
        await _indexingService.Index(scaperData.PageNumber, response.Data);
        scaperData.RowFetched = response.Data.Count();
    }

    private async Task GetCast(IEnumerable<TvShow> tvShows)
    {
        foreach (var tvShow in tvShows)
        {
            var response = await _mazeRestClient.GetTvShowCast(tvShow.Id);
            if (response?.Data == null || !response.IsSuccessful)
            {
                _logger.LogError(response.ErrorException, $"Failed to get Cast Information for {tvShow.Id} : {tvShow.Name}. {response.ErrorMessage}");
                continue;
            }
            tvShow.Casts = response.Data;
        }
    }
}
