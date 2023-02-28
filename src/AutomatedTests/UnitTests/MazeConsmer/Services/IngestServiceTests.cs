using Common;

namespace UnitTests.MazeConsmer.Services;

[TestClass]
public class IngestServiceTests
{
    [TestMethod]
    public async Task Ingest_WhenGetTvShowRestApiCallsFails_ShouldNotThrowException()
    {
        var service = new IngestServiceBuilder().WithGetTvShowCallReturnsNull().Build();
        try
        {
            await service.Ingest(new ScraperData());
        }
        catch 
        {
            Assert.Fail("Not supposed to throw exception.");
        }
    }

    [TestMethod]
    public async Task Ingest_WhenGetTvShowRestApiCallsReturnsData_ShouldGetCastInfo()
    {
        var service = new IngestServiceBuilder().WithGetTvShowCallReturnsData().Build();
        var data = new ScraperData();
        await service.Ingest(data);
        Assert.AreEqual(data.RowFetched, 1);
    }
}
