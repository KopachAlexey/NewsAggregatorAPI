using MediatR;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class AddCommentHandler : IRequestHandler<AddCommentCommand, Guid>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly CommentMapper _commentMapper;

        public AddCommentHandler(NewsAggregatorContext dbContext, CommentMapper commentMapper)
        {
            _dbContext = dbContext;
            _commentMapper = commentMapper;
        }

        public async Task<Guid> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = _commentMapper.CommentDTOTOEntity(request.CommentDTO);
            await _dbContext.Comments.AddAsync(comment, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return comment.Id;
        }
    }
}
