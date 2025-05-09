using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetNewsWithoutRatingQuery : IRequest<NewsDTO[]>
    {

    }
}
