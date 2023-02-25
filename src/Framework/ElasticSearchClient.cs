using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Framework;

public class ElasticSearchClient : IElasticSearchClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ElasticSearchClient> _logger;
    private const string IndexName = "tv-shows";

    public ElasticSearchClient(IConfiguration configuration, ILogger<ElasticSearchClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IEnumerable<T>> Search<T>(int pageNumber, int pageSize)
    {
        var response = await GetClient().SearchAsync<T>(x => x
        .Index(IndexName)
        .From(pageNumber)
        .Size(pageSize)
        .Query(q => q.MatchAll()));

        if (!response.IsValidResponse)
        {
            _logger.LogError("Error response from Elastic search. {ElasticError}", response.ElasticsearchServerError);
            return Enumerable.Empty<T>();
        }
        return response.Documents;
    }

    public async Task Index<T>(IEnumerable<T> items)
    {
        var client = GetClient();
        foreach (var item in items)
        {
            var response = await client.IndexAsync(item, request => request.Index(IndexName));
            if (!response.IsValidResponse)
                _logger.LogWarning("Indexing failed. {ElasticError}", response.ElasticsearchServerError);
        }
    }

    private ElasticsearchClient GetClient()
    {
        var settings = new ElasticsearchClientSettings(new Uri(_configuration["ElasticClientUrl"]))
                        .CertificateFingerprint(_configuration["FingerPrint"])
                        .Authentication(new BasicAuthentication(_configuration["EsUsername"], _configuration["EsPassword"]));
        return new ElasticsearchClient(settings);
    }
}
