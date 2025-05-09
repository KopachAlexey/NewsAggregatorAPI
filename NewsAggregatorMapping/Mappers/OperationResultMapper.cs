using NewsAggregatorCore.DTO;
using Riok.Mapperly.Abstractions;
using NewsAggregatorModels.Models;

namespace NewsAggregatorMapping.Mappers
{
    [Mapper]
    public partial class OperationMapper
    {
        [MapperIgnoreSource(nameof(OperationResultDTO.IsSuccessful))]
        public partial ErrorResponse ResultToResponse(OperationResultDTO operationResult);
    }
}
