using Common;
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

    public async Task<IEnumerable<TvShow>> GetTvShows(int pageNumber) => await CallRestApi<IEnumerable<TvShow>>($"{_apiUrl}shows?page={pageNumber}");

    public async Task<IEnumerable<Cast>> GetTvShowCast(int tvShowId) => await CallRestApi<IEnumerable<Cast>>($"{_apiUrl}shows/{tvShowId}/cast");

    private async Task<T?> CallRestApi<T>(string apiUrl) where T : class
    {
        using var restClient = new RestClient();
        var response = await restClient.ExecuteAsync<T>(new RestRequest(apiUrl));
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            _logger.LogWarning("Too many Request due to rate limiter. Waiting 10 seconds. {Response}", response.ErrorMessage);
            Thread.Sleep(TimeSpan.FromSeconds(10));
            response = await restClient.ExecuteAsync<T>(new RestRequest(apiUrl));
        }
        if(response.StatusCode == HttpStatusCode.OK)    
            return response?.Data;
        _logger.LogError(response.ErrorException, response.ErrorMessage);
        return null;
    }
}
