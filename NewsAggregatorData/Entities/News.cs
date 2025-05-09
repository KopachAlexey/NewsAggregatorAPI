namespace NewsAggregatorData.Entities
{
    public class News
    {
        private Guid _id;

        public Guid Id => _id;
        public string? Topic {  get; set; }
        public string Headline { get; set; }
        public string? Summary {get; set;}
        public string? Content { get; set; }
        public DateTimeOffset PublicationDate { get; set; }
        public string Url { get; set; }
        public string? ImageUrl { get; set; }
        public double? PositivityRate { get; set; }
        public Source? Source { get; set; }
        public int SourceId { get; set; }
        public List<Comment> Comments { get; set; } = new();
    }
}