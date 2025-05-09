using NewsAggregatorCore.DTO;

namespace NewsAggregatorServices.Abstracts
{
    public interface IRssNewsReader
    {
        public Task<NewsDTO[]> ReadAsync(SourceDTO source);
    }
}
