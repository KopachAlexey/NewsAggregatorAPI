using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetNewsCardsByRateQuery : IRequest<NewsCardDTO[]>
    {
        public double MinRate { get; init; }
        public int SkipNewsCount { get; init; }
        public int TakeNewsCount { get; init; }

    }
}
