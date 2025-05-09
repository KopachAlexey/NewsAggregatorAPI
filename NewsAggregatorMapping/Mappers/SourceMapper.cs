using NewsAggregatorData.Entities;
using NewsAggregatorCore.DTO;
using NewsAggregatorModels.Models;
using Riok.Mapperly.Abstractions;

namespace NewsAggregatorMapping.Mappers
{
    [Mapper]
    public partial class SourceMapper
    {
        [MapperIgnoreTarget(nameof(Source.Id))]
        public partial Source DtoToEntity(SourceDTO sourceDTO);

        public partial SourceDTO EntityToDto(Source source);

        public partial SourceResponse DtoToModel(SourceDTO sourceDTO);

    }
}
