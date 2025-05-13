using MediatR;
using Moq;
using NewsAggregatorCore.DTO;
using NewsAggregatorCQS.Querys;
using NewsAggregatorServices.Abstracts;
using NewsAggregatorServices.Implementations;

namespace NewsAggregatorTests
{
    public class CommentReactionServiceTests
    {
        readonly Mock<IMediator> _mediatorMock;
        readonly CommentReactionServices _commentReactionServices;

        public CommentReactionServiceTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _commentReactionServices = new CommentReactionServices(_mediatorMock.Object);

        }

        [Fact]
        public async Task GetCommentReactionsByReactionName_IfReactionsExists_ReturnReactions()
        {
            var commentId = Guid.NewGuid();
            var reactionId = Guid.NewGuid();
            var reactionName = "Like";
            var reactionsToComment = new UserCommentReactionDTO[]
            {
                new UserCommentReactionDTO
                {
                    UserId = Guid.NewGuid(),
                    CommentId = commentId,
                    ReactionId = reactionId,
                    ReactionName = reactionName,
                    UserLogin = "Login1"
                },
                new UserCommentReactionDTO
                {
                    UserId = Guid.NewGuid(),
                    CommentId = commentId,
                    ReactionId = reactionId,
                    ReactionName = reactionName,
                    UserLogin = "Login2"
                },
                new UserCommentReactionDTO
                {
                    UserId = Guid.NewGuid(),
                    CommentId = commentId,
                    ReactionId = reactionId,
                    ReactionName = reactionName,
                    UserLogin = "Login3"
                }
            };
            _mediatorMock.Setup(m =>
                m.Send(It.Is<GetReactionByNameQuery>(q => q.ReactionName == reactionName),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReactionDTO { ReactionName = reactionName, Id = reactionId });
            _mediatorMock.Setup(m =>
                m.Send(It.Is<GetUserCommentReactionsQuery>(q => q.ReactionId.Equals(reactionId)
                && q.CommentId.Equals(commentId)),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(reactionsToComment);
            var result = await _commentReactionServices.GetCommentReactionsByNameAsync(commentId, reactionName);
            Assert.NotNull(result);
            Assert.Equal(reactionsToComment.Length, result.Length);
            for (int i = 0; i < reactionsToComment.Length; i++)
            {
                Assert.Equal(reactionsToComment[i].ReactionName, result[i].ReactionName);
                Assert.Equal(reactionsToComment[i].ReactionId, result[i].ReactionId);
                Assert.Equal(reactionsToComment[i].UserLogin, result[i].UserLogin);
            }
        }

        [Fact]
        public async Task GetCommentReactionByUserId_IfReactionExists_ReturnReaction()
        {
            var userId = Guid.NewGuid();
            var commentId = Guid.NewGuid();
            var reactionToComment = new UserCommentReactionDTO
            {
                UserId = userId,
                CommentId = commentId,
                UserLogin = "Login3",
                ReactionName = "like"
            };
            _mediatorMock.Setup(m =>
                m.Send(It.Is<GetUserCommentReactionQuery>(q => q.UserId.Equals(userId)
                    && q.CommentId.Equals(commentId)),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(reactionToComment);
            var result = await _commentReactionServices.GetCommentReactionByUserIdAsync(commentId, userId);
            Assert.NotNull(result);
            Assert.Equal(reactionToComment.CommentId, result.CommentId);
            Assert.Equal(reactionToComment.UserId, result.UserId);
            Assert.Equal(reactionToComment.UserLogin, result.UserLogin);
            Assert.Equal(reactionToComment.ReactionName, result.ReactionName);
        }

        [Fact]
        public async Task AddNewReactionAsync_IfReactionAddded_ReturnPositiveAddReactionResult()
        {

        }

        [Fact]
        public async Task UpdateReactionAsync_IdReactionUpdated_ReturnPositiveOperationResult()
        {

        }


    }
}
