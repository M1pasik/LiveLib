using LiveLib.Api.Common;
using LiveLib.Application.Features.Reviews.CreateReview;
using LiveLib.Application.Features.Reviews.DeleteReview;
using LiveLib.Application.Features.Reviews.GetReviewById;
using LiveLib.Application.Features.Reviews.GetReviewsByBookId;
using LiveLib.Application.Features.Reviews.UpdateReview;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LiveLib.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Produces("application/json")]
    public class ReviewsController : ControllerApiBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(IMediator mediator, ILogger<ReviewsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("book/{bookId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<ReviewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByBookId(
            [FromRoute, Required] Guid bookId,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new GetReviewsByBookIdQuery(bookId), ct);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving reviews for book {BookId}", bookId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while retrieving reviews for book {bookId}"
                });
            }
        }

        [HttpGet("{reviewId:guid}")]
        [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetail(
            [FromRoute, Required] Guid reviewId,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new GetReviewByIdQuery(reviewId), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving review {ReviewId}", reviewId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while retrieving review {reviewId}"
                });
            }
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add(
            [FromBody, Required] CreateReviewCommand request,
            CancellationToken ct)
        {
            try
            {
                request.UserId = User.Id();
                var result = await _mediator.Send(request, ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review by user {UserId}", User.Id());
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An error occurred while creating review"
                });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute, Required] Guid id,
            [FromBody, Required] UpdateReviewDto updated,
            CancellationToken ct)
        {
            try
            {
                var reviewResult = await _mediator.Send(new GetReviewByIdQuery(id), ct);
                if (reviewResult.IsFailure)
                {
                    return ToActionResult(reviewResult);
                }

                if (reviewResult.Value!.UserId != User.Id())
                {
                    return Forbid();
                }

                var updateResult = await _mediator.Send(new UpdateReviewCommand(id, updated), ct);
                return ToActionResult(updateResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating review {ReviewId} by user {UserId}", id, User.Id());
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while updating review {id}"
                });
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromRoute, Required] Guid id,
            CancellationToken ct)
        {
            try
            {
                var reviewResult = await _mediator.Send(new GetReviewByIdQuery(id), ct);
                if (reviewResult.IsFailure)
                {
                    return ToActionResult(reviewResult);
                }

                if (reviewResult.Value!.UserId != User.Id() && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var result = await _mediator.Send(new DeleteReviewByIdCommand(id), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review {ReviewId} by user {UserId}", id, User.Id());
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while deleting review {id}"
                });
            }
        }
    }
}