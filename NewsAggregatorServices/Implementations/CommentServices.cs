using MediatR;
using NewsAggregatorCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorCQS.Commands;
using NewsAggregatorCQS.Querys;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class CommentServices : ICommentServices
    {
        readonly IMediator _mediator;

        public CommentServices(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Guid> AddCommentAsync(CommentDTO comment)
        {
            return await _mediator.Send(new AddCommentCommand { CommentDTO = comment });
        }

        public async Task<OperationResultDTO> DelCommentByIdAsync(Guid id)
        {
            bool isSuccessful = true;
            var messages = new List<string>();
            if((await _mediator.Send(new GetCommentByIdQuery { Id = id})) is null)
            {
                isSuccessful = false;
                messages.Add(NewsAggregatorMessages.CommentDoesNotExist);
            }
            else
                await _mediator.Send(new DelCommentByIdCommand { Id = id });
            return new OperationResultDTO
            {
                IsSuccessful = isSuccessful,
                Messages = messages
            };
        }

        public async Task<CommentDTO[]> GetCommentsByNewsIdAsync(Guid newsId)
        {
            return await _mediator.Send(new GetCommentsByNewsIdQuery { NewsId = newsId });
        }
    }
}
