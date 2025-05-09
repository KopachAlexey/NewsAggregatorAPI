
using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetAllSourceQuery: IRequest<SourceDTO[]>
    {
        
    }
}
