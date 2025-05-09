using HtmlAgilityPack;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorServices.Abstracts
{
    public abstract class SourceScrapper
    {
        private readonly HttpClient _httpClient;

        public SourceScrapper(HttpClient httpClient) => _httpClient = httpClient;

        protected void RemoveNodes(List<HtmlNodeCollection> nodesList)
        {
            foreach (var nodes in nodesList)
                if (nodes is not null)
                    foreach (var node in nodes)
                        node.Remove();
        }

        public async Task<NewsDTO[]> ScrapingNewsAsync(NewsDTO[] newsCollection)
        {
            foreach (var news in newsCollection)
            {
                var response = await _httpClient.GetAsync(news.Url);
                var htmlDoc = new HtmlDocument();
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    htmlDoc.Load(stream);
                    FillNews(htmlDoc, news);
                }
            }
            return newsCollection;
        }

        protected abstract void FillNews(HtmlDocument document, NewsDTO news);
    }
}
