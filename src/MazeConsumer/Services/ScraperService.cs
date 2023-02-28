using Common;
using MazeConsumer.DbContext;

namespace MazeConsumer.Services;

public class ScraperService : IScraperService
{
    private readonly IConfiguration _configuration;
    private readonly IIngestService _ingestService;
    private readonly IScrapperDbContext _dbContext;

    public ScraperService(IConfiguration configuration, IIngestService ingestService, IScrapperDbContext dbContext)
    {
        _configuration = configuration;
        _ingestService = ingestService;
        _dbContext = dbContext;
    }

    public async Task Scrap()
    {
        IsRunning = true;
        int numberOfThread = _configuration.GetValue<int>("NumberOfThread");
        var taskList = new List<Task>();
        while (true)
        {
            for (int i = 0; i < numberOfThread; i++)
            {
                var data = _dbContext.GetNextScraperData();
                taskList.Add(Task.Factory.StartNew(() => Process(data)));
            }
            Task.WaitAll(taskList.ToArray());
            if (_dbContext.Scrapers.Any(x => x.RowFetched == 0))
                break;
        }
        IsRunning= false;
    }

    private void Process(ScraperData data) => _ingestService.Ingest(data).Wait();

    public bool IsRunning { get; private set; }
}