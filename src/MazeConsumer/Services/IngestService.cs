using Common;
using MazeConsumer.DbContext;

namespace MazeConsumer.Services;

public class IngestService : IIngestService
{
    private readonly IMazeRestClient _mazeRestClient;
    private readonly IScrapperDbContext _dbContext;
    private readonly ILogger<IngestService> _logger;
    private readonly IElasticSearchClient _elasticSearchClient;

    public IngestService(IMazeRestClient mazeRestClient, IScrapperDbContext dbContext, IElasticSearchClient elasticSearchClient, ILogger<IngestService> logger)
    {
        _mazeRestClient = mazeRestClient;
        _dbContext = dbContext;
        _logger = logger;
        _elasticSearchClient = elasticSearchClient;
    }

    public async Task Ingest()
    {
        while (true)
        {
            var scraperData = _dbContext.GetNextScraperData();
            _logger.LogInformation("Processing Page - {Page}", scraperData.PageNumber);
            var response = await _mazeRestClient.GetTvShows(scraperData.PageNumber);
            if (response == null)
            {
                _logger.LogError($"Failed to fetch Tv Shows for page {scraperData.PageNumber}.");
                return;
            }
            _logger.LogInformation($"Tv Shows received: {response.Count()}");
            await GetCast(response);
            await _elasticSearchClient.Index(response);
            scraperData.RowFetched = response.Count();
            _dbContext.Save();
            _logger.LogInformation("Processing Page - {Page} - Completed. Row fetch {Rows}", scraperData.PageNumber, scraperData.RowFetched);
            if (!response.Any())
                break;
        }
    }

    private async Task GetCast(IEnumerable<TvShow> tvShows) => tvShows.ToList().ForEach(async tvShow =>
        {
            await GetCast(tvShow);
        });

    private async Task GetCast(TvShow tvShow)
    {
        _logger.LogInformation("Getting cast for {TvShow}", tvShow.Name);
        var response = await _mazeRestClient.GetTvShowCast(tvShow.Id);
        tvShow.Casts = response;
    }
}
