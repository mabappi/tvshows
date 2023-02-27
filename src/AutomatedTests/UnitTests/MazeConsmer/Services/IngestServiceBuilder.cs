using Common;
using MazeConsumer.Services;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.BaseLibrary;

namespace UnitTests.MazeConsmer.Services;

internal class IngestServiceBuilder : BuilderBase<IngestService>
{
    protected override IngestService BuildInternal()
    {
        return new IngestService(Get<IMazeRestClient>(), Get<IElasticSearchClient>(), Get<ILogger<IngestService>>());
    }

    internal IngestServiceBuilder WithGetTvShowCallReturnsData()
    {
        var mock = new Mock<IMazeRestClient>();
        IEnumerable<TvShow> result = new[] { new TvShow() };
        mock.Setup(x => x.GetTvShows(It.IsAny<int>())).Returns(Task.FromResult(result));
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
