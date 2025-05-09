using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetCommentsByNewsIdQuery : IRequest<CommentDTO[]>
    {
        public Guid NewsId { get; init; }
    }
}
