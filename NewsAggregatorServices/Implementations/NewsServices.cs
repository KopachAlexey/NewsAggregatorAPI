using NewsAggregatorCore.DTO;
using MediatR;
using NewsAggregatorCQS.Commands;
using NewsAggregatorServices.Abstracts;
using NewsAggregatorCQS.Querys;
using NewsAggregatorCore;
using NewsAggregatorModels.Models;

namespace NewsAggregatorServices.Implementations
{
    public class NewsServices : INewsServices
    {
        readonly IMediator _mediator;

        public NewsServices(IMediator mediator) => _mediator = mediator;

        public async Task DelAllAsync()
        {
            await _mediator.Send(new DelAllNewsCommand());
        }

        public async Task DelByIdAsync(Guid id)
        {
            await _mediator.Send(new DelNewsByIdCommand { Id = id });
        }

        public async Task<NewsDTO?> GetByIdAsync(Guid id)
        {
            return await _mediator.Send(new GetNewsByIdQuery { Id = id });
        }

        public async Task<OperationResultDTO> UpdateNewsRateByIdAsync(Guid id, double newRate)
        {
            var isSuccessful = true;
            var error = String.Empty;
            var messages = new List<string>();
            var fields = new List<string>();
            if (newRate > NewsAggregatorConstants.MaxNewsRate || newRate < NewsAggregatorConstants.MinNewsRate)
            {
                isSuccessful = false;
                error = OperationErrorsEnum.INCORRECT_NEWS_RATING.ToString();
                fields.Add(OperationFieldsEnum.MinRate.ToString());
                messages.Add(NewsAggregatorMessages.IncorrectRate);
            }
            await _mediator.Send(new UpdateNewsRateByIdCommand { NewRate = newRate, Id = id });
            return new OperationResultDTO
            {
                IsSuccessful = isSuccessful,
                Error = error,
                Messages = messages,
                Filds = fields
            };
        }
    }
}
