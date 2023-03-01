using Common;

namespace UnitTests.MazeConsmer.Services;

[TestClass]
public class IngestServiceTests
{
    [TestMethod]
    public async Task Ingest_WhenGetTvShowRestApiCallsFails_ShouldNotThrowException()
    {
        var service = new IngestServiceBuilder()
            .WithDbContext(null)
            .WithGetTvShowCallReturnsNull().Build();
        try
        {
            await service.Ingest();
        }
        catch 
        {
            Assert.Fail("Not supposed to throw exception.");
        }
    }

    [TestMethod]
    public async Task Ingest_WhenGetTvShowRestApiCallsReturnsData_ShouldGetCastInfo()
    {
        var data = new ScraperData { PageNumber= 1 };
        var service = new IngestServiceBuilder()
            .WithDbContext(data)
            .WithGetTvShowCallReturnsData().Build();
        await service.Ingest();
        Assert.AreEqual(data.RowFetched, 1);
    }
}
