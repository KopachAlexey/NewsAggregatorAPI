using NewsAggregatorCore.DTO;
using NewsAggregatorData.Entities;
using NewsAggregatorModels.Models;
using Riok.Mapperly.Abstractions;

namespace NewsAggregatorMapping.Mappers
{
    [Mapper]
    public partial class CommentMapper
    {
        public partial CommentDTO EntityToCommentDTO(Comment comment);

        [MapperIgnoreTarget(nameof(Comment.Id))]
        [MapperIgnoreTarget(nameof(Comment.News))]
        [MapperIgnoreTarget(nameof(Comment.User))]
        public partial Comment CommentDTOTOEntity(CommentDTO commentDTO);

        public partial CommentDTO AddCommentRequestTOCommentDTO(AddCommentRequest newComment);

        public CommentResponse CommentDTOToCommentResponse(CommentDTO commentDTO)
        {
            var commentResponse = MapCommentDTOToCommentResponse(commentDTO);
            commentResponse.UserRoleName = commentDTO.User?.Role?.RoleName ?? String.Empty;
            return commentResponse;
        }

        [MapProperty(nameof(CommentDTO.User.Login), nameof(CommentResponse.UserLogin))]
        private partial CommentResponse MapCommentDTOToCommentResponse(CommentDTO commentDTO);

        private string GetUserRoleName(RoleDTO? role)
        {
            return role?.RoleName ?? String.Empty;
        }
    }
}
