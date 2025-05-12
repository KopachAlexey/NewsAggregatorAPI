using FluentValidation;
using NewsAggregatorCore;
using NewsAggregatorModels.Models;

namespace NewsAggregatorModels.Validation
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator() 
        {
            RuleFor(u => u.Login)
                .Length(NewsAggregatorConstants.MinLoginLenth, NewsAggregatorConstants.MaxLoginLenth)
                .When(u => u.Login is not null);
            RuleFor(u => u.Password)
                .Length(NewsAggregatorConstants.MinPasswordLenth, NewsAggregatorConstants.MaxPasswordLenth);
            RuleFor(u => u.NewPassword)
                .Length(NewsAggregatorConstants.MinPasswordLenth, NewsAggregatorConstants.MaxPasswordLenth)
                .When(u => u.NewPassword is not null);
            RuleFor(u => u.Email)
                .EmailAddress()
                .When(u => u.Email is not null)
                .MinimumLength(NewsAggregatorConstants.MinEmailLenth)
                .When(u => u.Email is not null);
        }
    }
}
