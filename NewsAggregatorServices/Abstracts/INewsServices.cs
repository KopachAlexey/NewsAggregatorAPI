using NewsAggregatorCore.DTO;

namespace NewsAggregatorServices.Abstracts
{
    public interface INewsServices
    {
        Task<NewsDTO?> GetByIdAsync(Guid id);
        Task DelByIdAsync(Guid id);
        Task<OperationResultDTO> UpdateNewsRateByIdAsync(Guid id, double newRate);

        Task DelAllAsync();
    }
}
