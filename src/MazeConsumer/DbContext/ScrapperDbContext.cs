using Common;
using Newtonsoft.Json;

namespace MazeConsumer.DbContext;

public class ScrapperDbContext : IScrapperDbContext
{
    private readonly IConfiguration _configuration;
    private static object _lock = new object();
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

    public void Save()
    {
        lock(_lock ) 
        {
            File.WriteAllText(ScrapDataFileName,
                JsonConvert.SerializeObject(Scrapers.Where(x => x.RowFetched != 0)));
        }
    }

    public ScraperData GetNextScraperData()
    {
        ScraperData data;
        lock (_lock)
        {
            data = new ScraperData { PageNumber = Scrapers.Count + 1 };
            Scrapers.Add(data);
        }
        return data;
    }

    private string ScrapDataFileName => Path.Combine(_configuration["DataDirectory"] ?? "/data", "SrcaperData.json");
}
