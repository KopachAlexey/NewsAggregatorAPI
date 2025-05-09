namespace NewsAggregatorServices.Abstracts
{
    public interface ITextlemmatizer
    {
        Task<Dictionary<Guid, string[]>> GetLemmasFromTextsAsync(Dictionary<Guid, string> textById, CancellationToken cancellationToken);
    }
}
