using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Commands
{
    public class AddSourceCommand : IRequest
    {
       public SourceDTO SourceDTO { get; set; }
    }
}
