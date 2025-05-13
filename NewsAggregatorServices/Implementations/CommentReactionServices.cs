using MediatR;
using NewsAggregatorCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorCQS.Commands;
using NewsAggregatorCQS.Querys;
using NewsAggregatorModels.Models;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class CommentReactionServices : ICommentReactionServices
    {
        readonly IMediator _mediator;

        public CommentReactionServices(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<AddResourceResultDTO> AddReactionToCommentAsync(Guid commentId, Guid userId, string reactionName)
        {
            var operationResult = new OperationResultDTO { IsSuccessful = true };
            var reaction = await _mediator.Send(new GetReactionByNameQuery { ReactionName = reactionName });
            if(reaction is null)
                operationResult = GetNoSuchReactionResult();
            var reactionToComment = await _mediator.Send(new GetUserCommentReactionQuery { CommentId = commentId, UserId = userId });
            if(reactionToComment is not null)
                operationResult = GetReactionAlredyExistsResult();
            var userReactionToComment = new UserCommentReactionDTO
            {
                CommentId = commentId,
                UserId = userId,
                ReactionId = reaction.Id
            };
            var id = await _mediator.Send(new AddUserReactionToCommentCommand { NewUserReactionToComment = userReactionToComment });
            return new AddResourceResultDTO { Id = id, OperationResult = operationResult };
        }

        public async Task<UserCommentReactionDTO?> GetCommentReactionByUserIdAsync(Guid commentId, Guid userId)
        {
            return await _mediator.Send(new GetUserCommentReactionQuery { CommentId = commentId, UserId = userId });
        }

        public async Task<UserCommentReactionDTO[]> GetCommentReactionsByNameAsync(Guid commentId, string reactionName)
        {
            var reaction = await _mediator.Send(new GetReactionByNameQuery { ReactionName = reactionName });
            if (reaction is null)
                return Array.Empty<UserCommentReactionDTO>();
            return await _mediator.Send(new GetUserCommentReactionsQuery { CommentId = commentId, ReactionId = reaction.Id });
        }

        public async Task<OperationResultDTO> UpdateReactionToCommentAsync(Guid commentId, Guid userId, string reactionName)
        {
            var reaction = await _mediator.Send(new GetReactionByNameQuery { ReactionName = reactionName });
            if (reaction is null)
                return GetNoSuchReactionResult();
            var reactionToComment = await _mediator.Send(new GetUserCommentReactionQuery { CommentId = commentId, UserId = userId });
            if (reactionToComment is null)
                return GetReactionDontExistsResult();
            var userReactionToComment = new UserCommentReactionDTO
            {
                CommentId = commentId,
                UserId = userId,
                ReactionId = reaction.Id
            };
            await _mediator.Send(new UpdateUserReactionToCommentCommand { UserReactionToComment = userReactionToComment });
            return new OperationResultDTO { IsSuccessful = true };
        }

        private OperationResultDTO GetNoSuchReactionResult()
        {
            return new OperationResultDTO
            {
                IsSuccessful = false,
                Error = OperationErrorsEnum.NO_SUCH_REACTION.ToString(),
                Messages = new List<string> { NewsAggregatorMessages.NoSuchReaction },
                Filds = new List<string> { OperationFieldsEnum.ReactionName.ToString() }
            };
        }

        private OperationResultDTO GetReactionAlredyExistsResult()
        {
            return new OperationResultDTO
            {
                IsSuccessful = false,
                Error = OperationErrorsEnum.REACTION_ALREDY_EXISTS.ToString(),
                Messages = new List<string> { NewsAggregatorMessages.CommentReactionAlredyExists },
                Filds = new List<string> { OperationFieldsEnum.CommentId.ToString(), OperationFieldsEnum.UserId.ToString() }
            };
        }

        private OperationResultDTO GetReactionDontExistsResult()
        {
            return new OperationResultDTO
            {
                IsSuccessful = false,
                Error = OperationErrorsEnum.REACTION_DONT_EXISTS.ToString(),
                Messages = new List<string> { NewsAggregatorMessages.CommentReactionDontExists },
                Filds = new List<string> { OperationFieldsEnum.CommentId.ToString(), OperationFieldsEnum.UserId.ToString() }
            };
        }
    }
}
