using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorCore.DTO;
using NewsAggregatorMapping.Mappers;
using NewsAggregatorModels.Models;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentReactionController : ControllerBase
    {
        readonly ICommentReactionServices _commentReactionServices;
        readonly ILogger<CommentReactionController> _logger;
        readonly IValidator<ReactionToCommentRequest> _reactionToCommentValidator;
        readonly OperationMapper _operationMapper;

        public CommentReactionController(ICommentReactionServices commentReactionServices, 
            IValidator<ReactionToCommentRequest> reactionToCommentValidator, OperationMapper operationMapper,
            ILogger<CommentReactionController> logger)
        {
            _commentReactionServices = commentReactionServices;
            _reactionToCommentValidator = reactionToCommentValidator;
            _operationMapper = operationMapper;
            _logger = logger;
        }

        [HttpGet("get-comment-reactions-by-reaction-name")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCommentReactionDTO[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCommentReactionsByReactionName(Guid commentId, string reactionName)
        {
            try
            {
                var validationResult = _reactionToCommentValidator
                    .Validate(new ReactionToCommentRequest { ReactionName = reactionName }, opt =>
                    {
                        opt.IncludeProperties(r => r.ReactionName);
                    });
                if(!validationResult.IsValid)
                    return BadRequest();
                var commentReactions = await _commentReactionServices.GetCommentReactionsByNameAsync(commentId, reactionName);
                return Ok(commentReactions);
            }
            catch (Exception)
            {
                _logger.LogError($"Error while trying to get all reactions to comment with reaction name = {reactionName}, " +
                    $"commentId = {commentId}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("get-comment-reaction-by-user-id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCommentReactionDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCommentReactionByUserId(Guid commentId, Guid userId)
        {
            try
            {
                var commentReaction = await _commentReactionServices.GetCommentReactionByUserIdAsync(commentId, userId);
                return Ok(commentReaction);
            }
            catch (Exception)
            {
                _logger.LogError($"Error while trying to get user reaction to comment with id = {commentId}, " +
                    $"userId = {userId}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddReactionToComment([FromBody] ReactionToCommentRequest reactionToComment)
        {
            try
            {
                var validationResult = _reactionToCommentValidator.Validate(reactionToComment);
                if(!validationResult.IsValid)
                    return BadRequest();
                var addReactionResult = await _commentReactionServices.AddReactionToCommentAsync(reactionToComment.CommentId, 
                    reactionToComment.UserId, reactionToComment.ReactionName);
                if(addReactionResult.OperationResult.IsSuccessful)
                    return Created($"api/CommentReaction/{addReactionResult.Id}", new { Id = addReactionResult.Id });
                else
                    return Conflict(_operationMapper.ResultToResponse(addReactionResult.OperationResult));
            }
            catch (Exception)
            {
                _logger.LogError($"Error while trying to add reaction to comment with id = {reactionToComment.CommentId}, " +
                    $"userId = {reactionToComment.UserId}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCommentReaction([FromBody] ReactionToCommentRequest reactionToComment)
        {
            try
            {
                var validationResult = _reactionToCommentValidator.Validate(reactionToComment);
                if (!validationResult.IsValid)
                    return BadRequest();
                var updateReactionResult = await _commentReactionServices.UpdateReactionToCommentAsync(reactionToComment.CommentId,
                   reactionToComment.UserId, reactionToComment.ReactionName);
                if (updateReactionResult.IsSuccessful)
                    return NoContent();
                else
                    return Conflict(_operationMapper.ResultToResponse(updateReactionResult));
            }
            catch (Exception)
            {
                _logger.LogError($"Error while trying to update reaction to comment with id = {reactionToComment.CommentId}, " +
                   $"userId = {reactionToComment.UserId}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
