using NewsAggregatorCore.DTO;
using NewsAggregatorData.Entities;

namespace NewsAggregatorServices.Abstracts
{
    public interface ICommentServices
    {
        public Task<CommentDTO[]> GetCommentsByNewsIdAsync(Guid newsId);
        public Task<Guid> AddCommentAsync(CommentDTO comment);
        public Task<OperationResultDTO> DelCommentByIdAsync(Guid id);
    }
}
