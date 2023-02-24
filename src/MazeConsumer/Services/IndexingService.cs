using Newtonsoft.Json;
using RestSharp;

namespace MazeConsumer.Services;

public class IndexingService : IIndexingService
{
    private readonly string _storeDirectory;
    private readonly string _indexingServiceUrl;
    public IndexingService(IConfiguration configuration)
    {
        _storeDirectory = configuration["StoreDirectory"];
        _indexingServiceUrl = configuration["IndexingServiceUrl"];
    }
    public async Task Index(int pageNumber, IEnumerable<TvShow> shows)
    {
        await File.WriteAllTextAsync(Path.Combine(_storeDirectory, $"{pageNumber}.json"), JsonConvert.SerializeObject(shows));
        using var restClient = new RestClient();
        await restClient.ExecuteAsync(new RestRequest($"{_indexingServiceUrl}?pageNumber={pageNumber}"));
    }
}
