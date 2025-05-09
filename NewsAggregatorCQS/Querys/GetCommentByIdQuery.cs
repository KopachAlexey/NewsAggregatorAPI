using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetCommentByIdQuery : IRequest<CommentDTO?>
    {
        public Guid Id { get; init; }
    }
}
