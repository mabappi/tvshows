namespace UnitTests.TvShows;

[TestClass]
public class SearchServiceTests
{
    [TestMethod]
    public async Task Search_WhenPageNumberIsZero_ThenShouldReturnEmptyList()
    {
        var result = await new SearchServiceBuilder().Build().Search(0, 10);
        result.Should().NotBeNull().And.BeEmpty();
    }

    [TestMethod]
    public async Task Search_WhenPageSizeIsZero_ThenShouldReturnEmptyList()
    {
        var result = await new SearchServiceBuilder().Build().Search(10, 0);
        result.Should().NotBeNull().And.BeEmpty();
    }

    [TestMethod]
    public async Task Search_WhenPageNumberIsOneAndPageSizeIsTwo_ThenShouldReturnTwoItems()
    {
        var result = await new SearchServiceBuilder()
            .WithElasticClientReturningTvShows(2)
            .Build()
            .Search(1, 2);
        result
            .Should()
            .NotBeNull()
            .And.NotBeEmpty().And.HaveCount(2);
    }

}