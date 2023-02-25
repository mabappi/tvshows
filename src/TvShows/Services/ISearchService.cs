using Framework;

namespace TvShows.Services;

public interface ISearchService
{
    Task<IEnumerable<dynamic>> Search(int pageNumber, int pageSize);
}