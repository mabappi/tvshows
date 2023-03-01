using Common;
using Newtonsoft.Json;
using Polly;
using RestSharp;
using System.Net;

namespace MazeConsumer.Services;

public class MazeRestClient : IMazeRestClient
{
    private readonly string _apiUrl;
    private readonly ILogger<MazeRestClient> _logger;

    public MazeRestClient(IConfiguration configuration, ILogger<MazeRestClient> logger)
    {
        _apiUrl = configuration.GetValue<string>("ApiUrl") ?? "http://api.tvmaze.com/";
        _logger = logger;
    }

    public async Task<IEnumerable<TvShow>> GetTvShows(int pageNumber) 
        => await CallRestApiWithRetry<IEnumerable<TvShow>>($"{_apiUrl}shows?page={pageNumber}");

    public async Task<IEnumerable<Cast>> GetTvShowCast(int tvShowId) 
        => await CallRestApiWithRetry<IEnumerable<Cast>>($"{_apiUrl}shows/{tvShowId}/cast");

    private async Task<T> CallRestApiWithRetry<T>(string apiUrl) where T : class
    {
        var response = await PolicyHelper.GetPolicy().ExecuteAsync(async () => await CallRestApi(apiUrl));

        return JsonConvert.DeserializeObject<T>(response.Content);
    }

    private async Task<RestResponse> CallRestApi(string apiUrl) 
    {
        using var restClient = new RestClient(new RestClientOptions { MaxTimeout = 300000 });
        var response = await restClient.ExecuteAsync(new RestRequest(apiUrl) { Timeout = 300000 });
        if(!response.IsSuccessful) 
        {
            _logger.LogError(response.ErrorException, $"Status code: {response.StatusDescription}");
        }
        return response;
    }
}
