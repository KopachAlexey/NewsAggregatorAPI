using MediatR;
using NewsAggregatorCQS.Querys;
using NewsAggregatorCore.DTO;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class NewsPagginator : INewsPagginator
    {
        const int _skipSubtrahend = 1;
        readonly IMediator _mediator;

        public NewsPagginator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<NewsPageDTO> GetNewsPageAsync(double minRate, int pageNumber, int pageSize)
        {
            var skipNewsCount = (pageNumber - _skipSubtrahend) * pageSize;
            var newsCars = await _mediator.Send(new GetNewsCardsByRateQuery 
            { 
                MinRate = minRate, 
                SkipNewsCount = skipNewsCount, 
                TakeNewsCount = pageSize
            });
            var newsCount = await _mediator.Send(new GetNewsCountByRateQuery { MinRate = minRate });
            return new NewsPageDTO
            {
                NewsCards = newsCars,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNews = newsCount
            };

        }
    }
}
