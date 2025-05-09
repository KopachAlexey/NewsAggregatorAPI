
using System.Text.RegularExpressions;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class HtmlRemover : IHtmlRemover
    {
        private readonly string _fullDelTagRegex = @"<(script|style|button|a)[^>]*>.*?<\/\1>";
        private readonly string _imgTagRegex = @"<(img)[^>]*>";
        private readonly string _delTagRegex = @"<[^>]+>";
        private readonly string _htmlCommentRegex = @"<!--.*?-->";

        public string RemoveHtmlFromText(string text)
        {
            string textWithoutHtmlComment = Regex.Replace(text, _htmlCommentRegex, String.Empty, RegexOptions.Singleline);
            string textWithoutImg = Regex.Replace(textWithoutHtmlComment, _imgTagRegex, String.Empty, RegexOptions.IgnoreCase);
            string textWithoutNoContentTag = Regex.Replace(textWithoutImg, _fullDelTagRegex, String.Empty, RegexOptions.Singleline 
                | RegexOptions.IgnoreCase);
            return Regex.Replace(textWithoutNoContentTag, _delTagRegex, String.Empty);
        }
    }
}
