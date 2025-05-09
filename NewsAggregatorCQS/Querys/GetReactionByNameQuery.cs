using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetReactionByNameQuery : IRequest<ReactionDTO?>
    {
        public string ReactionName { get; init; }
    }
}
