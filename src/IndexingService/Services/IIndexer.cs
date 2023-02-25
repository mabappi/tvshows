namespace IndexingService.Services;

public interface IIndexer
{
    Task Index(int pageNumber);
}