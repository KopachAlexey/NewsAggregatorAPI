namespace NewsAggregatorCore.DTO
{
    public class NewsDTO
    {
        public Guid Id { get; init; }
        public string Headline { get; set; }
        public string? Summary { get; set; }
        public string? Content { get; set; }
        public DateTimeOffset PublicationDate { get; set; }
        public string Url { get; set; }
        public string? ImageUrl { get; set; }
        public double? PositivityRate { get; set; }
        public int SourceId { get; set; }
        public string SourceName { get; set; }
    }
}
