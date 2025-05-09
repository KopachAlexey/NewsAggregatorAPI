using MediatR;

namespace NewsAggregatorCQS.Querys
{
    public class GetNewsCountByRateQuery : IRequest<int>
    {
        public double MinRate { get; set; }
    }
}
