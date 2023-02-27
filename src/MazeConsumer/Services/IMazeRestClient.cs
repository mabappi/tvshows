using Common;
using RestSharp;

namespace MazeConsumer.Services;

public interface IMazeRestClient
{
    Task<RestResponse<IEnumerable<Cast>>> GetTvShowCast(int tvShowId);
    Task<RestResponse<IEnumerable<TvShow>>> GetTvShows(int pageNumber);
}