using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorMapping.Mappers;
using NewsAggregatorModels.Models;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        readonly ICommentServices _commentServices;
        readonly CommentMapper _commentMapper;
        readonly IValidator<AddCommentRequest> _commentValidator;

        public CommentsController(ICommentServices commentServices, CommentMapper commentMapper, IValidator<AddCommentRequest> commentValidator)
        {
            _commentServices = commentServices;
            _commentMapper = commentMapper;
            _commentValidator = commentValidator;
        }

        [HttpGet("get-comments-by-news-id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommentResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCommentsByNewsId(Guid newsId)
        {
            try
            {
                var comments = await _commentServices.GetCommentsByNewsIdAsync(newsId);
                var commentsResponse = comments.Select(c => _commentMapper.CommentDTOToCommentResponse(c));
                return Ok(commentsResponse);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpDelete("del-comment/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DelComment(Guid id)
        {
            try
            {
                var operationResult = await _commentServices.DelCommentByIdAsync(id);
                if (operationResult.IsSuccessful)
                    return NoContent();
                else
                    return BadRequest(operationResult.Messages);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPost("add-comment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddComment( [FromBody] AddCommentRequest commentRequest)
        {
            try
            {
                var validationResult = _commentValidator.Validate(commentRequest);
                if (!validationResult.IsValid)
                    return BadRequest();
                var commentDTO = _commentMapper.AddCommentRequestTOCommentDTO(commentRequest);
                var commentId = await _commentServices.AddCommentAsync(commentDTO);
                return Created($"api/Comments/{commentId}", new { Id = commentId });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
           
        }
    }
}
