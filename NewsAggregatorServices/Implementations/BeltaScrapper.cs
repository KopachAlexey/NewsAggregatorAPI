using HtmlAgilityPack;
using NewsAggregatorCore.DTO;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class BeltaScrapper : SourceScrapper
    {
        public BeltaScrapper(HttpClient httpClient) : base(httpClient) { }

        protected override void FillNews(HtmlDocument document, NewsDTO news)
        {
            var dellNodesList = new List<HtmlNodeCollection>();
            dellNodesList.Add(document.DocumentNode.SelectNodes("//script"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//div[contains(@class,'news_tags_block')]"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//iframe"));
            RemoveNodes(dellNodesList);
            news.Content = document.DocumentNode.SelectSingleNode("//div[@class='js-mediator-article']").InnerHtml;
            var mainImage = document.DocumentNode.SelectSingleNode("//div[@class='main_img']//img");
            news.ImageUrl = mainImage is null ? "~" : mainImage.GetAttributeValue("src", "");
        }
    }
}
