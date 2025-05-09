namespace NewsAggregatorModels.Models
{
    public class ErrorResponse
    {
        public string Error { get;set;}
        public IEnumerable<string> Messages { get; set; }
        public IEnumerable<string> Filds { get; set; }

    }
}
