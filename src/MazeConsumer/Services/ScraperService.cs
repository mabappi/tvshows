using MazeConsumer.Models;
using Newtonsoft.Json;

namespace MazeConsumer.Services;

public class ScraperService : IScraperService
{
    private readonly IConfiguration _configuration;
    private readonly IIngestService _ingestService;
    private readonly IList<ScaperData> _scaperList;
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

    private IList<ScaperData> LoadScraperData() => 
        File.Exists(ScrapDataFileName) 
        ? JsonConvert.DeserializeObject<IList<ScaperData>>(File.ReadAllText(ScrapDataFileName)) ?? new List<ScaperData>()
        : new List<ScaperData>();

    private void SaveScraperData() => File.WriteAllText(ScrapDataFileName, JsonConvert.SerializeObject(_scaperList));

    public bool IsRunning { get; private set; }

    private string ScrapDataFileName => Path.Combine(_configuration["StoreDirectory"] ?? "/tvmazeData", "SrcaperData.json");
}