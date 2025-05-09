using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetUserCommentReactionsHandler : IRequestHandler<GetUserCommentReactionsQuery, UserCommentReactionDTO[]>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly UserCommentReactionMapper _commentReactionMapper;

        public GetUserCommentReactionsHandler(NewsAggregatorContext dbContext, UserCommentReactionMapper commentReactionMapper)
        {
            _dbContext = dbContext;
            _commentReactionMapper = commentReactionMapper;
        }

        public async Task<UserCommentReactionDTO[]> Handle(GetUserCommentReactionsQuery request, CancellationToken cancellationToken)
        {
            var commentReactions = await _dbContext.UserCommentReactions
                .Include(u => u.Reaction)
                .Include(u => u.User)
                .Where(u => u.CommentId.Equals(request.CommentId) && u.ReactionId.Equals(request.ReactionId))
                .ToArrayAsync();
            if (commentReactions is null)
                return Array.Empty<UserCommentReactionDTO>();
            return commentReactions.Select(c => _commentReactionMapper.EntityToDto(c)).ToArray();
        }
    }
}
