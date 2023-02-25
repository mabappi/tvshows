namespace Framework;

public interface IIndexingService
{
    Task Index(int pageNumber, IEnumerable<TvShow> shows);
}