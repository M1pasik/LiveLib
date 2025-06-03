using LiveLib.Api.Common;
using LiveLib.Application.Features.BookPublishers.CreateBookPublisher;
using LiveLib.Application.Features.BookPublishers.DeleteBookPublisher;
using LiveLib.Application.Features.BookPublishers.GetBookPublisherById;
using LiveLib.Application.Features.BookPublishers.GetBookPublishers;
using LiveLib.Application.Features.BookPublishers.UpdateBookPublisher;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LiveLib.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    [Produces("application/json")]
    public class BookPublishersController : ControllerApiBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookPublishersController> _logger;

        public BookPublishersController(IMediator mediator, ILogger<BookPublishersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookPublisherDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            try
            {
                var publishers = await _mediator.Send(new GetBookPublishersQuery(), ct);
                return Ok(publishers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all book publishers");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An error occurred while retrieving book publishers"
                });
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BookPublisherDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDetail(
            [FromRoute, Required] Guid id,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new GetBookPublisherByIdQuery(id), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book publisher with ID: {PublisherId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while retrieving book publisher with ID {id}"
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Add(
            [FromBody, Required] CreateBookPublisherCommand request,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(request, ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book publisher");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An error occurred while creating book publisher"
                });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute, Required] Guid id,
            [FromBody, Required] UpdateBookPublisherDto updated,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new UpdateBookPublisherCommand(id, updated), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book publisher with ID: {PublisherId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while updating book publisher with ID {id}"
                });
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
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
                var result = await _mediator.Send(new DeleteBookPublisherCommand(id), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book publisher with ID: {PublisherId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while deleting book publisher with ID {id}"
                });
            }
        }
    }
}