using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetCommentByIdHandler : IRequestHandler<GetCommentByIdQuery, CommentDTO?>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly CommentMapper _commentMapper;

        public GetCommentByIdHandler(NewsAggregatorContext dbContext, CommentMapper commentMapper)
        {
            _dbContext = dbContext;
            _commentMapper = commentMapper;
        }

        public async Task<CommentDTO?> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await _dbContext.Comments
                .Include(c => c.User)
                .ThenInclude(u => u.Role)
                .Include(c => c.News)
                .Include(c => c.UserCommentReactions)
                .SingleOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken);
            return comment is null ? null : _commentMapper.EntityToCommentDTO(comment);
        }
    }
}
