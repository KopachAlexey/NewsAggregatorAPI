using NewsAggregatorCore.DTO;

namespace NewsAggregatorServices.Abstracts
{
    public interface ISourceServices
    {
        public Task<SourceDTO[]> GetAllAsync();
    }
}
