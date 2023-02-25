using Framework;

namespace MazeConsumer.Services;

public interface IIngestService
{
    Task Ingest(ScraperData scaperData);
}