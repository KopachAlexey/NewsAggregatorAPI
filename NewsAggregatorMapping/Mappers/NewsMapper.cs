using NewsAggregatorData.Entities;
using NewsAggregatorCore.DTO;
using Riok.Mapperly.Abstractions;

namespace NewsAggregatorMapping.Mappers
{
    [Mapper]
    public partial class NewsMapper
    {
        [MapperIgnoreTarget(nameof(News.Id))]
        [MapperIgnoreTarget(nameof(News.Source))]
        public partial News NewsDtoToEntity(NewsDTO newsDTO);

        [MapProperty(nameof(News.Source.Name), nameof(NewsCardDTO.SourceName))]
        public partial NewsDTO EntityToNewsDto(News news);

        [MapProperty(nameof(News.Source.Name), nameof(NewsCardDTO.SourceName))]
        public partial NewsCardDTO EntityToNewsCardDto(News news);

    }
}
