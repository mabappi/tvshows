using Common;

namespace TvShows.Services;

public class SearchService : ISearchService
{
    private readonly IElasticSearchClient _elasticSearchClient;

    public SearchService(IElasticSearchClient elasticSearchClient) => _elasticSearchClient = elasticSearchClient;

    public async Task<IEnumerable<dynamic>> Search(int pageNumber, int pageSize) =>
        pageNumber == 0 || pageSize == 0
        ? Enumerable.Empty<dynamic>()
        : (await _elasticSearchClient.Search<TvShow>(pageNumber, pageSize))
        .Select(x => new
        {
            id = x.Id,
            name = x.Name,
            cast = x.Casts.OrderBy(o => o.Person.Birthday)
            .Select(c => new { id = c.Person.Id, name = c.Person.Name, birthday = c.Person.Birthday })
        });
}
