namespace IndexingService.Services;

public interface IElasticSearchClient
{
    Task Index(int pageNumber);
}