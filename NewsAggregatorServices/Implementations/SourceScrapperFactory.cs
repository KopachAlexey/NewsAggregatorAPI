

using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class SourceScrapperFactory : ISourceScrapperFactory
    {
        readonly IEnumerable<SourceScrapper> _sourceScrappers;

        IReadOnlyDictionary<string, Type> _sourceNameAndScrapperTypeMatch => new Dictionary<string, Type>
        {
            ["Belta"] = typeof(BeltaScrapper),
            ["Telegraf"] = typeof(TelegrafScrapper),
            ["Ria"] = typeof(RiaScrapper)
        };

        public SourceScrapperFactory(IEnumerable<SourceScrapper> sourceScrappers)
        {
            _sourceScrappers = sourceScrappers;
        }

        public SourceScrapper? GetScrapper(string sourceName)
        {
            _sourceNameAndScrapperTypeMatch.TryGetValue(sourceName, out Type? scrapperType);
            return scrapperType is null ? null : _sourceScrappers.FirstOrDefault(s => s.GetType() == scrapperType);
        }
    }
}
