using System.Diagnostics;

namespace MazeConsumer.Services;

public class ScraperService : IScraperService
{
    private readonly IConfiguration _configuration;
    private readonly IIngestService _ingestService;
    private readonly ILogger<ScraperService> _logger;

    public ScraperService(IConfiguration configuration, IIngestService ingestService, ILogger<ScraperService> logger)
    {
        _configuration = configuration;
        _ingestService = ingestService;
        _logger = logger;
    }

    public async Task Scrap()
    {
        IsRunning = true;
        int numberOfThread = _configuration.GetValue<int>("NumberOfThread");
        var taskList = new List<Task>();
        var timer = new Stopwatch();
        timer.Start();
        for (int i = 0; i < numberOfThread; i++)
        {
            taskList.Add(Task.Factory.StartNew(Process));
        }
        Task.WaitAll(taskList.ToArray());
        timer.Stop();
        _logger.LogInformation("Completion time {Time}", timer.Elapsed.Minutes);
        IsRunning = false;
    }

    private void Process() => _ingestService.Ingest().Wait();

    public bool IsRunning { get; private set; }
}