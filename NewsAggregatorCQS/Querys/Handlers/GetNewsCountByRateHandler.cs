using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetNewsCountByRateHandler : IRequestHandler<GetNewsCountByRateQuery, int>
    {
        readonly NewsAggregatorContext _dbContext;

        public GetNewsCountByRateHandler(NewsAggregatorContext dbContext) => _dbContext = dbContext;

        public async Task<int> Handle(GetNewsCountByRateQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.News.Where(n => n.PositivityRate >= request.MinRate).CountAsync();
        }
    }
}
