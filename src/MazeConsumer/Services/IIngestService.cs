using MazeConsumer.Models;

namespace MazeConsumer.Services;

public interface IIngestService
{
    Task Ingest(ScaperData scaperData);
}