namespace UnitTests.MazeConsmer.Services
{
    [TestClass]
    public class ScrapperDbContextTests
    {
        [TestMethod]
        public void GetNextScraperData_ShouldReturnScraperDataWithPageNumber()
        {
            var dbContext = new ScrapperDbContextBuilder().Build();
            var scraperData = dbContext.GetNextScraperData();
            scraperData.Should().NotBeNull();
            Assert.AreEqual(0, scraperData.RowFetched);
            Assert.IsTrue(scraperData.PageNumber > 0);
        }

        [TestMethod]
        public void Save_ShouldSaveIgnoreDataWithRowsZero()
        {
            var dbContext = new ScrapperDbContextBuilder().Build();
            var scraperData = dbContext.GetNextScraperData();
            dbContext.Save();
            dbContext = new ScrapperDbContextBuilder().Build();
            Assert.IsFalse(dbContext.Scrapers.Any(x => x.RowFetched == 0));
        }

        [TestMethod]
        public void Save_ShouldSaveDataWithRowCount()
        {
            var dbContext = new ScrapperDbContextBuilder().Build();
            var scraperData = dbContext.GetNextScraperData();
            scraperData.RowFetched = 100;
            dbContext.Save();
            dbContext = new ScrapperDbContextBuilder().Build();
            Assert.IsFalse(dbContext.Scrapers.Any(x => x.RowFetched == 0));
        }
    }
}
