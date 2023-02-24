using Newtonsoft.Json;
using RestSharp;

namespace MazeConsumer.Services;

public class IndexingService : IIndexingService
{
    private readonly string _storeDirectory;
    private readonly string _indexingServiceUrl;
    private readonly ILogger<IndexingService> _logger;

    public IndexingService(IConfiguration configuration, ILogger<IndexingService> logger)
    {
        _storeDirectory = configuration["StoreDirectory"];
        _indexingServiceUrl = configuration["IndexingServiceUrl"];
        _logger = logger;
    }
    public async Task Index(int pageNumber, IEnumerable<TvShow> shows)
    {
        try
        {
            await File.WriteAllTextAsync(Path.Combine(_storeDirectory, $"{pageNumber}.json"), JsonConvert.SerializeObject(shows));
            using var restClient = new RestClient();
            await restClient.ExecuteAsync(new RestRequest($"{_indexingServiceUrl}?pageNumber={pageNumber}"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
