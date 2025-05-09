using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetUserCommentReactionHandler : IRequestHandler<GetUserCommentReactionQuery, UserCommentReactionDTO?>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly UserCommentReactionMapper _commentReactionMapper;

        public GetUserCommentReactionHandler(NewsAggregatorContext dbContext, UserCommentReactionMapper commentReactionMapper)
        {
            _dbContext = dbContext;
            _commentReactionMapper = commentReactionMapper;
        }

        public async Task<UserCommentReactionDTO?> Handle(GetUserCommentReactionQuery request, CancellationToken cancellationToken)
        {
            var userCommentReaction = await _dbContext.UserCommentReactions
                .Include(u => u.Reaction)
                .Include(u => u.User)
                .SingleOrDefaultAsync(u => u.UserId.Equals(request.UserId) && u.CommentId.Equals(request.CommentId));
            if (userCommentReaction is null)
                return null;
            return _commentReactionMapper.EntityToDto(userCommentReaction);
        }
    }
}
