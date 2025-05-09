using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetNewsByIdQuery : IRequest<NewsDTO?>
    {
        public Guid Id { get; init; }
    }
}
