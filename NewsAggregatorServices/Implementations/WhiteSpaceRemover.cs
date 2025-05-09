using System.Text.RegularExpressions;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class WhiteSpaceRemover : IWhiteSpaceRemover
    {
        private readonly string _removerPattern = @"\s+";
        private readonly string _replacement = " ";
        public string RemoveWhiteSpaceFromText(string text)
        {
            return Regex.Replace(text.TrimStart().TrimEnd(), _removerPattern, _replacement);
        }
    }
}
