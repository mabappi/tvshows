namespace Framework
{
    public interface IElasticSearchClient
    {
        Task Index<T>(IEnumerable<T> items);
        Task<IEnumerable<T>> Search<T>(int pageNumber, int pageSize);
    }
}