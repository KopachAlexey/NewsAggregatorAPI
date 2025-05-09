namespace NewsAggregatorCore.DTO
{
    public class NewsPageDTO
    {
        public IEnumerable<NewsCardDTO> NewsCards { get; set; } = new List<NewsCardDTO>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalNews { get; set; }
        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalNews / PageSize);
    }
}
