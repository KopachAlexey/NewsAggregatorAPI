using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace NewsAggregatorCQS.Commands
{
    public class UpdateUserNewsRateByIdCommand : IRequest
    {
        public Guid UserId { get; init; }
        public double NewNewsMinRate {  get; init; }
    }
}
