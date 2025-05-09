using MediatR;
using NewsAggregatorCQS.Commands;
using NewsAggregatorCQS.Querys;
using NewsAggregatorCore.DTO;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class NewsAggregator : INewsAggregator
    {
        readonly IMediator _mediator;
        readonly IRssNewsReader _rssNewsReader;
        readonly ISourceScrapperFactory _sourceScrapperFactory;

        public NewsAggregator(IMediator mediator, IRssNewsReader rssNewsReader, ISourceScrapperFactory sourceScrapperFactory)
        {
            _mediator = mediator;
            _rssNewsReader = rssNewsReader;
            _sourceScrapperFactory = sourceScrapperFactory;
        }

        public async Task AggregateNewsFromSourceAsync(SourceDTO source)
        {
            var scrapper = _sourceScrapperFactory.GetScrapper(source.Name);
            if (scrapper is null)
                throw new ArgumentException("There is no scraper for this source");
            var news = await _rssNewsReader.ReadAsync(source);
            var uniqueNews = await _mediator.Send(new FilterUniqueNewsQuery { NewsDTOs = news });
            var newNews = await scrapper.ScrapingNewsAsync(uniqueNews);
            await _mediator.Send(new AddNewsCommands { NewsDTOs = newNews });
        }
    }
}
