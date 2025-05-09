using FluentValidation;
using NewsAggregatorCore;
using NewsAggregatorModels.Models;

namespace NewsAggregatorModels.Validation
{
    public class UserValidator : AbstractValidator<AddUserRequest>
    {
        public UserValidator()
        {
            RuleFor(u => u.Login)
                .MinimumLength(NewsAggregatorConstants.MinLoginLenth)
                .MaximumLength(NewsAggregatorConstants.MaxLoginLenth);
            RuleFor(u => u.Password)
                .MinimumLength(NewsAggregatorConstants.MinPasswordLenth)
                .MaximumLength(NewsAggregatorConstants.MaxPasswordLenth);
            RuleFor(u => u.Email)
                .EmailAddress()
                .MinimumLength(NewsAggregatorConstants.MinEmailLenth);
        }
    }
}
