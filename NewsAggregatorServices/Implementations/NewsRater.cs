using MediatR;
using NewsAggregatorCQS.Commands;
using NewsAggregatorCQS.Querys;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class NewsRater : INewsRater
    {
        private readonly IMediator _mediator;
        private readonly ITextlemmatizer _textlemmatizer;
        private readonly ILemmasRater _lemmasRater;
        private IHtmlRemover _htmlRemover;
        private IWhiteSpaceRemover _whiteSpaceRemover;

        public NewsRater(IMediator mediator, ITextlemmatizer textlemmatizer, ILemmasRater lemmasRater, 
            IHtmlRemover htmlRemover, IWhiteSpaceRemover whiteSpaceRemover)
        {
            _mediator = mediator;
            _textlemmatizer = textlemmatizer;
            _lemmasRater = lemmasRater;
            _htmlRemover = htmlRemover;
            _whiteSpaceRemover = whiteSpaceRemover;
        }

        public async Task RateNewsAsync(CancellationToken cancellationToken)
        {
            var newsWithoutRate = await _mediator.Send(new GetNewsWithoutRatingQuery());
            var newsTextById = newsWithoutRate
                .Select(n => new KeyValuePair<Guid, string>(n.Id, 
                    _whiteSpaceRemover.RemoveWhiteSpaceFromText(_htmlRemover.RemoveHtmlFromText(n.Content))))
                .ToDictionary();
            var newsLemmasById = await _textlemmatizer.GetLemmasFromTextsAsync(newsTextById, cancellationToken);
            var newsRateById = _lemmasRater.RateLemmas(newsLemmasById);
            await _mediator.Send(new UpdateNewsRatingCommand(newsRateById));
        }
    }
}
