namespace NewsAggregatorCore.DTO
{
    public class OperationResultDTO
    {
        public bool IsSuccessful;
        public string Error { get; set; }
        public IEnumerable<string> Messages { get; set; }
        public IEnumerable<string> Filds { get; set; }
    }
}
