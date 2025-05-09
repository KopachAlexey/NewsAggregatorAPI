namespace NewsAggregatorModels.Models
{
    public class GetNewsPageRequest
    {
        public double MinRate { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}
