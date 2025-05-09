using MediatR;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class AddUserReactionToCommentHandler : IRequestHandler<AddUserReactionToCommentCommand, Guid>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly UserCommentReactionMapper _reactionMapper;

        public AddUserReactionToCommentHandler(NewsAggregatorContext dbContext, UserCommentReactionMapper reactionMapper)
        {
            _dbContext = dbContext;
            _reactionMapper = reactionMapper;
        }

        public async Task<Guid> Handle(AddUserReactionToCommentCommand request, CancellationToken cancellationToken)
        {
            var newUserReaction = _reactionMapper.DtoToEntity(request.NewUserReactionToComment);
            await _dbContext.UserCommentReactions.AddAsync(newUserReaction);
            await _dbContext.SaveChangesAsync();
            return newUserReaction.Id;
        }
    }
}
