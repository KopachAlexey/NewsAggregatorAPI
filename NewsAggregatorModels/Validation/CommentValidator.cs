using FluentValidation;
using NewsAggregatorModels.Models;

namespace NewsAggregatorModels.Validation
{
    public class CommentValidator : AbstractValidator<AddCommentRequest>
    {
        public CommentValidator()
        {
            RuleFor(c => c.NewsId).NotEmpty();
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.Text).NotEmpty();
            RuleFor(c => c.CreationDate).NotEmpty();
        }
    }
}
