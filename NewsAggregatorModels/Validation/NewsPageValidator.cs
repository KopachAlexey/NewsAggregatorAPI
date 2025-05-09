using FluentValidation;
using NewsAggregatorCore;
using NewsAggregatorModels.Models;

namespace NewsAggregatorModels.Validation
{
    public class NewsPageValidator : AbstractValidator<GetNewsPageRequest>
    {
        public NewsPageValidator()
        {
            RuleFor(p => p.PageNumber).GreaterThanOrEqualTo(NewsAggregatorConstants.DefaultPageNumber);
            RuleFor(p => p.PageSize).GreaterThanOrEqualTo(NewsAggregatorConstants.DefaultPageSize);
            RuleFor(p => p.MinRate).InclusiveBetween(NewsAggregatorConstants.MinNewsRate, NewsAggregatorConstants.MaxNewsRate);
        }
    }
}
