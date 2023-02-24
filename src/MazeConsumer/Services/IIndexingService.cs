namespace MazeConsumer.Services;

public interface IIndexingService
{
    Task Index(int pageNumber, IEnumerable<TvShow> shows);
}