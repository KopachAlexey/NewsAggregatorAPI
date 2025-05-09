using MediatR;

namespace NewsAggregatorCQS.Commands
{
    public class UpdateNewsRatingCommand : IRequest
    {
        private readonly Dictionary<Guid, double> _ratingById = new Dictionary<Guid, double>();

        public UpdateNewsRatingCommand(Dictionary<Guid, double> ratingById)
        {
            _ratingById = ratingById;
        }

        public IReadOnlyDictionary<Guid, double> RatingById => _ratingById;
    }
}
