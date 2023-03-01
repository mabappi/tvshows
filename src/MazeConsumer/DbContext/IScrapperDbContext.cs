using Common;

namespace MazeConsumer.DbContext;

public interface IScrapperDbContext
{
    IList<ScraperData> Scrapers { get; }
    ScraperData GetNextScraperData();
    void Save();
}