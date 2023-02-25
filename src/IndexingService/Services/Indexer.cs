using Framework;
using Newtonsoft.Json;

namespace IndexingService.Services;

public class Indexer : IIndexer
{
    private readonly IElasticSearchClient _elasticSearchClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<Indexer> _logger;

    public Indexer(IElasticSearchClient elasticSearchClient, IConfiguration configuration, ILogger<Indexer> logger)
    {
        _elasticSearchClient = elasticSearchClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task Index(int pageNumber)
    {
        var tvShows = LoadData(pageNumber);
        if (tvShows == null)
        {
            return;
        }

        await _elasticSearchClient.Index(tvShows);
    }

    private IEnumerable<TvShow> LoadData(int pageNumber)
    {
        string fileName = Path.Combine(_configuration["StoreDirectory"] ?? "data", $"{pageNumber}.json");
        if (!File.Exists(fileName))
        {
            _logger.LogWarning("Unable to index. {File} not found.", fileName);
            return null;
        }
        return JsonConvert.DeserializeObject<IEnumerable<TvShow>>(File.ReadAllText(fileName));
    }
}