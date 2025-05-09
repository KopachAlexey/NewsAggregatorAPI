using NewsAggregatorCore.DTO;
using NewsAggregatorData.Entities;
using Riok.Mapperly.Abstractions;

namespace NewsAggregatorMapping.Mappers
{
    [Mapper]
    public partial class ReactionMapper
    {
        [MapperIgnoreTarget(nameof(Reaction.Id))]
        public partial Reaction DtoToEntity(ReactionDTO reactionDTO);
        public partial ReactionDTO EntityToDto(Reaction reaction);
    }
}
