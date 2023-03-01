using Common;
using MazeConsumer.DbContext;
using MazeConsumer.Services;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.BaseLibrary;

namespace UnitTests.MazeConsmer.Services;

internal class IngestServiceBuilder : BuilderBase<IngestService>
{
    protected override IngestService BuildInternal()
    {
        return new IngestService(Get<IMazeRestClient>(), Get<IScrapperDbContext>(), Get<IElasticSearchClient>(), Get<ILogger<IngestService>>());
    }

    internal IngestServiceBuilder WithGetTvShowCallReturnsData()
    {
        var mock = new Mock<IMazeRestClient>();
        IEnumerable<TvShow> result = new[] { new TvShow() };
        mock.SetupSequence(x => x.GetTvShows(It.IsAny<int>()))
            .Returns(Task.FromResult(result))
            .Returns(Task.FromResult(Enumerable.Empty<TvShow>()));
        WithMock(mock);
        return this;
    }

    internal IngestServiceBuilder WithDbContext(ScraperData data) 
    {
        var mock = new Mock<IScrapperDbContext>();
        mock.SetupSequence(x => x.GetNextScraperData())
            .Returns(data ?? new ScraperData { PageNumber= 1 })
            .Returns(new ScraperData { PageNumber = 2 });
        WithMock(mock);
        return this; 
    }

    internal IngestServiceBuilder WithGetTvShowCallReturnsNull()
    {
        var mock = new Mock<IMazeRestClient>();
        IEnumerable<TvShow> result = null;
        mock.Setup(x => x.GetTvShows(It.IsAny<int>())).Returns(Task.FromResult(result));
        WithMock(mock);
        return this;
    }
}
