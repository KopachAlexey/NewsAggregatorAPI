using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class FilterUniqueNewsQuery : IRequest<NewsDTO[]>
    {
        public IEnumerable<NewsDTO> NewsDTOs { get; set; } = new List<NewsDTO>();
    }
}
