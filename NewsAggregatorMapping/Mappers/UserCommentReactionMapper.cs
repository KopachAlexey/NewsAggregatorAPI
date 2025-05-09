using NewsAggregatorCore.DTO;
using NewsAggregatorData.Entities;
using Riok.Mapperly.Abstractions;

namespace NewsAggregatorMapping.Mappers
{
    [Mapper]
    public partial class UserCommentReactionMapper
    {
        [MapProperty(nameof(UserCommentReaction.Reaction.ReactionName), nameof(UserCommentReactionDTO.ReactionName))]
        [MapProperty(nameof(UserCommentReaction.User.Login), nameof(UserCommentReactionDTO.UserLogin))]
        public partial UserCommentReactionDTO EntityToDto(UserCommentReaction userCommentReaction);

        [MapperIgnoreTarget(nameof(UserCommentReaction.Id))]
        public partial UserCommentReaction DtoToEntity(UserCommentReactionDTO userCommentReactionDTO);
    }
}
