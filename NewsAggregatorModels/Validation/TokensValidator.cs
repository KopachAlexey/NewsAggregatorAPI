using FluentValidation;
using NewsAggregatorModels.Models;

namespace NewsAggregatorModels.Validation
{
    public class TokensValidator : AbstractValidator<UpdateTokensRequest>
    {
        public TokensValidator()
        {
            RuleFor(t => t.AccessToken).NotEmpty();
            RuleFor(t => t.RefreshToken).NotEmpty();
        }
    }
}
