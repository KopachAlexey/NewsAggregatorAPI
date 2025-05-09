

namespace NewsAggregatorServices.Abstracts
{
    public interface ISourceScrapperFactory
    {
        public SourceScrapper? GetScrapper(string sourceName);
    }
}
