using MazeConsumer.Services;
using Moq;

namespace UnitTests.MazeConsmer.Services;

[TestClass]
public class ScraperServiceTests
{
    [TestMethod]
    public async Task Scrap_WhenNumberOfThreadSetToOne_IngestWillBeCalledOnce()
    {
        var ingestServiceMock = new Mock<IIngestService>();
        var service = new ScraperServiceBuilder()
            .WithConfigurationReturning(1)
            .WithMock(ingestServiceMock)
            .Build();
        await service.Scrap();
        ingestServiceMock.Verify(x => x.Ingest(), Times.Once(), "Should execute only once");
    }

    [TestMethod]
    public async Task Scrap_WhenNumberOfThreadSetToTen_IngestWillBeCalledTenTimes()
    {
        var ingestServiceMock = new Mock<IIngestService>();
        var service = new ScraperServiceBuilder()
            .WithConfigurationReturning(10)
            .WithMock(ingestServiceMock)
            .Build();
        await service.Scrap();
        ingestServiceMock.Verify(x => x.Ingest(), Times.Exactly(10), "Should execute 10 times");
    }
}
