
using HtmlAgilityPack;
using NewsAggregatorCore.DTO;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class RiaScrapper : SourceScrapper
    {
        public RiaScrapper(HttpClient httpClient) : base(httpClient) {}

        protected override void FillNews(HtmlDocument document, NewsDTO news)
        {
            var dellNodesList = new List<HtmlNodeCollection>();
            dellNodesList.Add(document.DocumentNode.SelectNodes("//script"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//iframe"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//div[contains(@data-type,'banner')]"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//div[contains(@data-type,'article')]"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//div[contains(@data-type,'victorina')]"));
            RemoveNodes(dellNodesList);
            news.Content = document.DocumentNode.SelectSingleNode("//div[@class='article__body js-mediator-article mia-analytics']").InnerHtml;
            var mainImage = document.DocumentNode.SelectSingleNode("//div[@class='article__announce']//img");
            var mainVideo = document.DocumentNode.SelectSingleNode("//div[@class='article__announce']//video");
            news.ImageUrl = mainImage is null ? 
                (mainVideo is null? "~" : mainVideo.GetAttributeValue("poster", "")) :
                mainImage.GetAttributeValue("src", "");
        }
    }
}
