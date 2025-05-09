namespace NewsAggregatorCore.DTO
{
    public class NewsCardDTO
    {
        public Guid Id { get; set; }
        public string Headline { get; set; }
        public DateTimeOffset PublicationDate { get; set; }
        public string Url { get; set; }
        public string? ImageUrl { get; set; }
        public double? PositivityRate { get; set; }
        public string? SourceName { get; set; }
    }
}
