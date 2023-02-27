using Common;
using Moq;
using TvShows.Services;
using UnitTests.BaseLibrary;

namespace UnitTests.TvShows;

public class SearchServiceBuilder : BuilderBase<SearchService>
{
    protected override SearchService BuildInternal() => new SearchService(Get<IElasticSearchClient>());

    internal SearchServiceBuilder WithElasticClientReturningTvShows(int numberOfTvShows)
    {
        var elasticClient = new Mock<IElasticSearchClient>();
        elasticClient
            .Setup(x => x.Search<TvShow>(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task.FromResult( GetTvShows(numberOfTvShows)));
        WithMock(elasticClient);
        return this;
    }

    private IEnumerable<TvShow> GetTvShows(int numberOfShows) 
    {
        for (int i = 0; i < numberOfShows; i++)
        {
            yield return new TvShow
            {
                Id = i,
                Name = i.ToString(),
                Casts = new[]
                {
                    new Cast
                    {
                        Person = new Person
                    {
                        Id = i,
                        Name = i.ToString(),
                        Birthday = DateTime.Now.AddDays(i).ToString()
                    }
                    }
                }
            };
        }
    }
}
