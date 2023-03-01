using Polly;
using RestSharp;
using System.Net;

namespace MazeConsumer.Services;

public class PolicyHelper
{
    static IAsyncPolicy<RestResponse> _resilienceStrategy;
    public static IAsyncPolicy<RestResponse> GetPolicy()
    {
        if(_resilienceStrategy != null)
            return _resilienceStrategy;

        IAsyncPolicy<RestResponse> limit = Policy
            .RateLimitAsync<RestResponse>(20, TimeSpan.FromSeconds(1), 20);

        IAsyncPolicy<RestResponse> retry = Policy
            .HandleResult<RestResponse>(result => result.StatusCode == HttpStatusCode.TooManyRequests)
            .OrResult(r => r.StatusCode == HttpStatusCode.ServiceUnavailable)
            .Or<Polly.RateLimit.RateLimitRejectedException>()
            .WaitAndRetryForeverAsync((retryNum) => {
                return TimeSpan.FromSeconds(1);
            });

        _resilienceStrategy = Policy.WrapAsync(retry, limit);

        return _resilienceStrategy;
    }
}
