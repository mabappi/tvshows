using MazeConsumer.Models;
using Newtonsoft.Json;

namespace MazeConsumer.Services;

public class ScraperService : IScraperService
{
    private readonly ILogger<ScraperService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IIngestService _ingestService;
    private IList<ScaperData> _scaperList;
    private static object _lockObject = new object();

    public ScraperService(ILogger<ScraperService> logger, IConfiguration configuration, IIngestService ingestService)
    {
        _logger = logger;
        _configuration = configuration;
        _ingestService = ingestService;
        LoadScraperData();
    }

    public async Task Scrap()
    {
        int numberOfThread = _configuration.GetValue<int>("NumberOfThread");
        var taskList = new List<Task>();
        for (int i = 0; i < numberOfThread; i++)
        {
            taskList.Add(Task.Factory.StartNew(ProcessScrap));
        }
        await Task.WhenAll(taskList);
    }

    private void ProcessScrap()
    {
        while (true)
        {
            var data = GetNext();
            _ingestService.Ingest(data);
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

    private ScaperData GetNext()
    {
        lock (_lockObject)
        {
            var data = new ScaperData { PageNumber = _scaperList.Count + 1 };
            _scaperList.Add(data);
            SaveScraperData();
            return data;
        }
    }

    private void LoadScraperData()
    {
        if (File.Exists(ScrapDataFileName))
            _scaperList = JsonConvert.DeserializeObject<IList<ScaperData>>(File.ReadAllText(ScrapDataFileName));
        else
            _scaperList = new List<ScaperData>();
    }

    private void SaveScraperData() => File.WriteAllText(ScrapDataFileName, JsonConvert.SerializeObject(_scaperList));

    private string ScrapDataFileName => Path.Combine(_configuration["StoreDirectory"], "SrcaperData.json");
}