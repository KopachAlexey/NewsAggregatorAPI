using MediatR;
using Moq;
using NewsAggregatorCore.DTO;
using NewsAggregatorCQS.Commands;
using NewsAggregatorCQS.Querys;
using NewsAggregatorData.Entities;
using NewsAggregatorServices.Abstracts;
using NewsAggregatorServices.Implementations;

namespace NewsAggregatorTests
{
    public class NewsServiceTests
    {
        readonly NewsServices _newsServices;
        readonly Mock<IMediator> _mediatorMock;

        public NewsServiceTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _newsServices = new NewsServices(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetNewsByIdAsync_IfNewsExists_ReturnNews()
        {
            var random = new Random();
            var newsId = Guid.NewGuid();
            var news = new NewsDTO
            {
                Id = newsId,
                Content = "Content",
                ImageUrl = "ImageUrl",
                PublicationDate = DateTimeOffset.UtcNow,
                Headline = "Headline"
            };
            _mediatorMock.Setup(m =>
                m.Send(It.Is<GetNewsByIdQuery>(q => q.Id.Equals(newsId)),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(news);
            var result = await _newsServices.GetByIdAsync(newsId);
            Assert.NotNull(result);
            Assert.Equal(news.Id, result.Id);
            Assert.Equal(news.PublicationDate, result.PublicationDate);
            Assert.Equal(news.ImageUrl, result.ImageUrl);
            Assert.Equal(news.Content, result.Content);
            Assert.Equal(news.Headline, result.Headline);
        }

        [Fact]
        public async Task GetNewsByIdAsync_IfNewsDoesNotExists_ReturnNull()
        {
            var random = new Random();
            var newsId = Guid.NewGuid();
            var news = new NewsDTO
            {
                Id = newsId,
                Content = "Content",
                ImageUrl = "ImageUrl",
                PublicationDate = DateTimeOffset.UtcNow,
                Headline = "Headline"
            };
            NewsDTO nullResult = null;
            _mediatorMock.Setup(m =>
                m.Send(It.Is<GetNewsByIdQuery>(q => q.Id.Equals(newsId)),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(nullResult);
            var result = await _newsServices.GetByIdAsync(Guid.NewGuid());
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateNewsRateByIdAsync_IfNewsUpdated_ReturnPositiveOperationResult()
        {
            var random = new Random();
            var newsId = Guid.NewGuid();
            var newNewsRate = random.NextDouble();
            var positiveOperationResult = new OperationResultDTO { IsSuccessful = true };
            _mediatorMock.Setup(m =>
                m.Send(It.Is<UpdateNewsRateByIdCommand>(c => c.Id.Equals(newsId)
                    && c.NewRate == newNewsRate),
                It.IsAny<CancellationToken>()));
            var result = await _newsServices.UpdateNewsRateByIdAsync(newsId, newNewsRate);
            Assert.NotNull(result);
            Assert.Equal(positiveOperationResult.IsSuccessful, result.IsSuccessful);
        }

    }
}
