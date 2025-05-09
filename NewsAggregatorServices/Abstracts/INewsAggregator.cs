

using NewsAggregatorCore.DTO;

namespace NewsAggregatorServices.Abstracts
{
    public interface INewsAggregator
    {
        public Task AggregateNewsFromSourceAsync(SourceDTO source);
    }
}
