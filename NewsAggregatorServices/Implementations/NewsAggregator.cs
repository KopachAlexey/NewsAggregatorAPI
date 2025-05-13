using MediatR;
using NewsAggregatorCQS.Commands;
using NewsAggregatorCQS.Querys;
using NewsAggregatorCore.DTO;
using NewsAggregatorServices.Abstracts;
using Microsoft.Extensions.Logging;

namespace NewsAggregatorServices.Implementations
{
    public class NewsAggregator : INewsAggregator
    {
        readonly IMediator _mediator;
        readonly IRssNewsReader _rssNewsReader;
        readonly ISourceScrapperFactory _sourceScrapperFactory;
        readonly ILogger<NewsAggregator> _logger;

        public NewsAggregator(IMediator mediator, IRssNewsReader rssNewsReader, 
            ISourceScrapperFactory sourceScrapperFactory, ILogger<NewsAggregator> logger)
        {
            _mediator = mediator;
            _rssNewsReader = rssNewsReader;
            _sourceScrapperFactory = sourceScrapperFactory;
            _logger = logger;
        }

        public async Task AggregateNewsFromSourceAsync(SourceDTO source)
        {
            try
            {
                var scrapper = _sourceScrapperFactory.GetScrapper(source.Name);
                if (scrapper is null)
                    throw new ArgumentException("There is no scraper for this source");
                var news = await _rssNewsReader.ReadAsync(source);
                var uniqueNews = await _mediator.Send(new FilterUniqueNewsQuery { NewsDTOs = news });
                var newNews = await scrapper.ScrapingNewsAsync(uniqueNews);
                await _mediator.Send(new AddNewsCommands { NewsDTOs = newNews });
                _logger.LogInformation($"{newNews.Length} new news from {source.Name} have been successfully aggregated");
            }
            catch (Exception)
            {
                _logger.LogError($"Error while trying to aggregate news from source, source id = {source.Id}" +
                    $"source name = {source.Name}");
            }
            
        }
    }
}
