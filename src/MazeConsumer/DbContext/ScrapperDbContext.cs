using Common;
using Newtonsoft.Json;

namespace MazeConsumer.DbContext;

public class ScrapperDbContext : IScrapperDbContext
{
    private readonly IConfiguration _configuration;

    public ScrapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        Scrapers = Load();
    }

    public IList<ScraperData> Scrapers { get; private set; }

    private IList<ScraperData> Load() =>
        File.Exists(ScrapDataFileName)
        ? JsonConvert.DeserializeObject<IList<ScraperData>>(File.ReadAllText(ScrapDataFileName)) ?? new List<ScraperData>()
        : new List<ScraperData>();

    public void Save() => File.WriteAllText(
        ScrapDataFileName,
        JsonConvert.SerializeObject(Scrapers.Where(x => x.RowFetched != 0)));

    public ScraperData GetNextScraperData()
    {
        var data = new ScraperData { PageNumber = Scrapers.Count + 1 };
        Scrapers.Add(data);
        Save();
        return data;
    }

    private string ScrapDataFileName => Path.Combine(_configuration["DataDirectory"] ?? "/data", "SrcaperData.json");
}
