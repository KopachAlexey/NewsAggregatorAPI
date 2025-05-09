namespace NewsAggregatorServices.Abstracts
{
    public interface ILemmasRater
    {
        Dictionary<Guid, double> RateLemmas(Dictionary<Guid, string[]> lemmasById);
    }
}
