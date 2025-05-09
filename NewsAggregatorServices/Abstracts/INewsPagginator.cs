using NewsAggregatorCore.DTO;

namespace NewsAggregatorServices.Abstracts
{
    public interface INewsPagginator
    {
        Task<NewsPageDTO> GetNewsPageAsync(double minRate, int pageNumber, int pageSize);
    }
}
