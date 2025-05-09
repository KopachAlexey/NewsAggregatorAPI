using FluentValidation;
using NewsAggregatorModels.Models;

namespace NewsAggregatorModels.Validation
{
    public class ReactionToCommentRequestValidator : AbstractValidator<ReactionToCommentRequest>
    {
        public ReactionToCommentRequestValidator()
        {
            RuleFor(r => r.ReactionName).NotEmpty();
        }
    }
}
