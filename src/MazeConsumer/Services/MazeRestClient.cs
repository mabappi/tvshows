using Common;
using RestSharp;
using System.Net;

namespace MazeConsumer.Services;

public class MazeRestClient : IMazeRestClient
{
    private readonly string _apiUrl;

    public MazeRestClient(IConfiguration configuration) => _apiUrl = configuration.GetValue<string>("ApiUrl") ?? "http://api.tvmaze.com/";

    public async Task<IEnumerable<TvShow>> GetTvShows(int pageNumber) => await CallRestApi<IEnumerable<TvShow>>($"{_apiUrl}shows?page={pageNumber}");

    public async Task<IEnumerable<Cast>> GetTvShowCast(int tvShowId) => await CallRestApi<IEnumerable<Cast>>($"{_apiUrl}shows/{tvShowId}/cast");

    private async Task<T?> CallRestApi<T>(string apiUrl) where T : class
    {
        using var restClient = new RestClient();
        var response = await restClient.ExecuteAsync<T>(new RestRequest(apiUrl));
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            Thread.Sleep(TimeSpan.FromSeconds(10));
            response = await restClient.ExecuteAsync<T>(new RestRequest(apiUrl));
        }
        if(response.StatusCode == HttpStatusCode.OK)    
            return response?.Data;
        
        return null;
    }
}
