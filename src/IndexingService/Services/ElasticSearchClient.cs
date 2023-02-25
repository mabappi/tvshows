using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Framework;
using Newtonsoft.Json;

namespace IndexingService.Services;

public class ElasticSearchClient : IElasticSearchClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ElasticsearchClient> _logger;

    public ElasticSearchClient(IConfiguration configuration, ILogger<ElasticsearchClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task Index(int pageNumber)
    {
        var tvShows = LoadData(pageNumber);
        if(tvShows == null)
        {
            return;
        }

        var client = GetClient();
        
        foreach(var tvShow in tvShows)
        {
            await client.IndexAsync(tvShow, request => request.Index("TvShows"));
        }
    }

    private IEnumerable<TvShow> LoadData(int pageNumber) 
    {
        string fileName = Path.Combine(_configuration["StoreDirectory"] ?? "tvmazeData", $"{pageNumber}.json");
        if(!File.Exists(fileName))
        {
            _logger.LogWarning("Unable to index. {File} not found.", fileName);
            return null;
        }
        return JsonConvert.DeserializeObject<IEnumerable<TvShow>>(fileName);
    }


    private ElasticsearchClient GetClient() 
        => new ElasticsearchClient(new ElasticsearchClientSettings(new Uri(_configuration["ElasticClientUrl"] ?? ""))
    .CertificateFingerprint(_configuration["FingerPrint"] ?? "")
    .Authentication(new BasicAuthentication(_configuration["Username"] ?? "", _configuration["Password"] ?? "")));
}
