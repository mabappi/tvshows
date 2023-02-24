using MazeConsumer.Models;

namespace MazeConsumer.Services
{
    public interface IScraperService
    {
        Task Scrap(ScaperData scaperData);
    }
}