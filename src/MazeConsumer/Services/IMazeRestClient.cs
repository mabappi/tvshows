using Common;

namespace MazeConsumer.Services;

public interface IMazeRestClient
{
    Task<IEnumerable<Cast>> GetTvShowCast(int tvShowId);
    Task<IEnumerable<TvShow>> GetTvShows(int pageNumber);
}