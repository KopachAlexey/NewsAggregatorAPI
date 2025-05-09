namespace NewsAggregatorServices.Abstracts
{
    public interface INewsRater
    {
        Task RateNewsAsync(CancellationToken cancellationToken);
    }
}
