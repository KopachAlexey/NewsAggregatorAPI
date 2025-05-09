using NewsAggregatorCore.DTO;

namespace NewsAggregatorServices.Abstracts
{
    public interface ICommentReactionServices
    {
        public Task<AddResourceResultDTO> AddReactionToCommentAsync(Guid commentId, Guid userId, string reactionName);
        public Task<UserCommentReactionDTO[]> GetCommentReactionsAsync(Guid commentId, string reactionName);
        public Task<OperationResultDTO> UpdateReactionToCommentAsync(Guid commentId, Guid userId, string reactionName);
        public Task<UserCommentReactionDTO?> GetCommentReactionAsync(Guid commentId, Guid userId);
    }
}
