using MediatR;
using Moq;
using NewsAggregatorCore.DTO;
using NewsAggregatorCQS.Commands;
using NewsAggregatorCQS.Querys;
using NewsAggregatorServices.Implementations;

namespace NewsAggregatorTests
{
    public class CommentServiceTests
    {
        readonly CommentServices _commentService;
        readonly Mock<IMediator> _mediatorMock;

        public CommentServiceTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _commentService = new CommentServices(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetCommentsByNewsIdAsync_IfNewsExists_ReturnsComments()
        {
            var newsId = Guid.NewGuid();
            var comments = new CommentDTO[]
            {
                new CommentDTO
                {
                    Id = Guid.NewGuid(),
                    NewsId = newsId,
                    Text = "Comment1",
                    CreationDate = DateTimeOffset.UtcNow
                },
                new CommentDTO
                {
                    Id = Guid.NewGuid(),
                    NewsId = newsId,
                    Text = "Comment2",
                    CreationDate = DateTimeOffset.UtcNow
                },
                new CommentDTO
                {
                    Id = Guid.NewGuid(),
                    NewsId = newsId,
                    Text = "Comment3",
                    CreationDate = DateTimeOffset.UtcNow
                }
            };
            _mediatorMock.Setup(m => 
                m.Send(It.Is<GetCommentsByNewsIdQuery>(q => q.NewsId.Equals(newsId)), 
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(comments);
            var result = await _commentService.GetCommentsByNewsIdAsync(newsId);
            Assert.NotNull(result);
            Assert.Equal(comments.Length, result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.Equal(comments[i].NewsId, result[i].NewsId);
                Assert.Equal(comments[i].Id, result[i].Id);
                Assert.Equal(comments[i].Text, result[i].Text);
                Assert.Equal(comments[i].CreationDate, result[i].CreationDate);
            }
        }

        [Fact]
        public async Task GetCommentsByNewsIdAsync_IfNewsDoesNotExists_ReturnsEmptyCommentArray()
        {
            var newsId = Guid.NewGuid();
            var existsNewsId = Guid.NewGuid();
            _mediatorMock.Setup(m =>
                m.Send(It.Is<GetCommentsByNewsIdQuery>(q => !q.NewsId.Equals(existsNewsId)),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<CommentDTO>());
            var result = await _commentService.GetCommentsByNewsIdAsync(newsId);
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public async Task AddCommentAsync_WhenCommentAdded_ReturnCommentId()
        {
            var newComment = new CommentDTO
            {
                Id = Guid.NewGuid(),
                NewsId = Guid.NewGuid(),
                Text = "Comment1",
                CreationDate = DateTimeOffset.UtcNow
            };
            _mediatorMock.Setup(m =>
                m.Send(It.Is<AddCommentCommand>(c => c.CommentDTO.CreationDate.Equals(newComment.CreationDate) &&
                    c.CommentDTO.Text == newComment.Text && c.CommentDTO.NewsId.Equals(newComment.NewsId)),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(newComment.Id);
            var result = await _commentService.AddCommentAsync(newComment);
            Assert.Equal(newComment.Id, result);
        }
    }
}
