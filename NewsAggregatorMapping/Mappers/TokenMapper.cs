using NewsAggregatorCore.DTO;
using NewsAggregatorData.Entities;
using Riok.Mapperly.Abstractions;

namespace NewsAggregatorMapping.Mappers
{
    [Mapper]
    public partial class TokenMapper
    {
        [MapperIgnoreTarget(nameof(RefreshToken.Id))]
        [MapperIgnoreTarget(nameof(RefreshToken.User))]
        public partial RefreshToken RefreshTokeDTOToEntity(RefreshTokenDTO tokenDTO);

        public partial RefreshTokenDTO EntityToRefreshTokeDTO(RefreshToken token);
    }
}
