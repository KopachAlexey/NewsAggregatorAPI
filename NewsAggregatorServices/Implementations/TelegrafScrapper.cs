using HtmlAgilityPack;
using NewsAggregatorCore.DTO;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class TelegrafScrapper : SourceScrapper
    {
        public TelegrafScrapper(HttpClient httpClient) : base(httpClient) { }

        protected override void FillNews(HtmlDocument document, NewsDTO news)
        {
            var dellNodesList = new List<HtmlNodeCollection>();
            dellNodesList.Add(document.DocumentNode.SelectNodes("//script"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//iframe"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//aside"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//blockquote"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//div[contains(@class,'zalipashka')]"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//div[@class='my-3']"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//div[contains(@class,'embed-container')]"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//div[contains(@class,'pikachu asdadgawf d-none d-md-block')]"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//div[contains(@class,'pikachu asdadgawf aj_no mb-3 mb-md-0 d-block d-md-none')]"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//div[contains(@class,'pikachu asdadgawf d-block d-md-none')]"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//div[contains(@class,'w-100 mb-3')]"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//div[contains(@class,'w-100 block-title d-flex justify-content-between align-items-center mb-3 content-vis-auto')]"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//div[contains(@class,'w-100 text-posta')]//div[contains(@class,'row')]"));
            dellNodesList.Add(document.DocumentNode.SelectNodes("//div[contains(@class,'pikachu pustoj aj_no w-100 mb-3 d-block d-md-none')]"));
            RemoveNodes(dellNodesList);
            news.Content = document.DocumentNode.SelectSingleNode("//div[@class='w-100 text-posta']").InnerHtml;
            var mainImage = document.DocumentNode.SelectSingleNode("//picture[@class='mb-0 w-100']//img");
            news.ImageUrl = mainImage is null ? "~" : mainImage.GetAttributeValue("src", "");
        }
    }
}
