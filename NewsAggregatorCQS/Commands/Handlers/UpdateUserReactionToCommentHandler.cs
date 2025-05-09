using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class UpdateUserReactionToCommentHandler : IRequestHandler<UpdateUserReactionToCommentCommand>
    {
        readonly NewsAggregatorContext _dbContext;

        public UpdateUserReactionToCommentHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(UpdateUserReactionToCommentCommand request, CancellationToken cancellationToken)
        {
            var userReactionToComment = await _dbContext.UserCommentReactions
                .SingleOrDefaultAsync(u => u.UserId.Equals(request.UserReactionToComment.UserId)
                && u.CommentId.Equals(request.UserReactionToComment.CommentId));
            if (userReactionToComment is null)
                return;
            userReactionToComment.ReactionId = request.UserReactionToComment.ReactionId;
            await _dbContext.SaveChangesAsync();
        }
    }
}
