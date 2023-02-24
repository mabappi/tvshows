namespace MazeConsumer.Services;

public interface IScraperService
{
    Task Scrap();
    bool IsRunning { get; }
}