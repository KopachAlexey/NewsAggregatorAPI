using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Commands
{
    public class AddNewsCommands :IRequest
    {
        public IEnumerable<NewsDTO> NewsDTOs { get; set; } = new List<NewsDTO>();
    }
}
