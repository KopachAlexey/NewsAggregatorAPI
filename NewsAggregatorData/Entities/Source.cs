namespace NewsAggregatorData.Entities
{
    public class Source
    {
        private int _id;

        public int Id => _id;
        public string Name { get; set; }
        public string RssUrl { get; set; }
        public List<News> News { get; set; } = new();
    }
}