using Microsoft.Extensions.Logging;
using NewsAggregatorCore.DTO;
using NewsAggregatorServices.Abstracts;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsAggregatorServices.Services
{
    public class RssNewsReader : IRssNewsReader
    {
        readonly ILogger<RssNewsReader> _logger;

        public RssNewsReader(ILogger<RssNewsReader> logger)
        {
            _logger = logger;
        }

        public async Task<NewsDTO[]> ReadAsync(SourceDTO source)
        {
            try
            {
                using (var xmlReader = XmlReader.Create(source.RssUrl))
                {
                    var rssFormatter = new Rss20FeedFormatter();
                    await Task.Run(() => rssFormatter.ReadFrom(xmlReader));
                    var rssItems = rssFormatter.Feed.Items;
                    var news = rssItems
                        .Select(i => new NewsDTO
                        {
                            Url = i.Links[0].Uri.ToString(),
                            Headline = i.Title.Text,
                            Summary = i.Summary is null ? "" : i.Summary.Text,
                            PublicationDate = DateTimeOffset.UtcNow,
                            SourceId = source.Id
                        })
                        .ToArray();
                    return news;
                }
            }
            catch (Exception)
            {
                _logger.LogError($"Error when reading rss from {source.RssUrl}");
                throw new Exception($"Read rss from {source.RssUrl} error");
            }
        }
    }
}
