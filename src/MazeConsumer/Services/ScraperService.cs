using Common;
using Newtonsoft.Json;

namespace MazeConsumer.Services;

public class ScraperService : IScraperService
{
    private readonly IConfiguration _configuration;
    private readonly IIngestService _ingestService;
    private readonly IList<ScraperData> _scaperList;
    private static object _lockObject = new object();

    public ScraperService(IConfiguration configuration, IIngestService ingestService)
    {
        _configuration = configuration;
        _ingestService = ingestService;
        _scaperList = LoadScraperData();
    }

    public async Task Scrap()
    {
        IsRunning = true;
        int numberOfThread = _configuration.GetValue<int>("NumberOfThread");
        var taskList = new List<Task>();
        for (int i = 0; i < numberOfThread; i++)
        {
            taskList.Add(Task.Factory.StartNew(ProcessScrap));
        }
        await Task.WhenAll(taskList);
        IsRunning= false;
    }

    private async Task ProcessScrap()
    {
        while (true)
        {
            var data = GetNextPage();
            
            await _ingestService.Ingest(data);
            if (data.RowFetched == 0)
            {
                lock (_lockObject)
                {
                    _scaperList.Remove(data);
                    SaveScraperData();
                }
                break;
            }
        }
    }

    private ScraperData GetNextPage()
    {
        lock (_lockObject)
        {
            var data = new ScraperData { PageNumber = _scaperList.Count + 1 };
            _scaperList.Add(data);
            SaveScraperData();
            return data;
        }
    }

    private IList<ScraperData> LoadScraperData() => 
        File.Exists(ScrapDataFileName) 
        ? JsonConvert.DeserializeObject<IList<ScraperData>>(File.ReadAllText(ScrapDataFileName)) ?? new List<ScraperData>()
        : new List<ScraperData>();

    private void SaveScraperData() => File.WriteAllText(ScrapDataFileName, JsonConvert.SerializeObject(_scaperList));

    public bool IsRunning { get; private set; }

    private string ScrapDataFileName => Path.Combine(_configuration["DataDirectory"] ?? "/data", "SrcaperData.json");
}