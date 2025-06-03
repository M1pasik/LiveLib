using LiveLib.Api.Common;
using LiveLib.Application.Features.Collections.AddBookToCollection;
using LiveLib.Application.Features.Collections.CreateCollection;
using LiveLib.Application.Features.Collections.DeleteCollection;
using LiveLib.Application.Features.Collections.GetCollectionById;
using LiveLib.Application.Features.Collections.RemoveBookFromCollection;
using LiveLib.Application.Features.Collections.UpdateCollection;
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
    public class CollectionsController : ControllerApiBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CollectionsController> _logger;

        public CollectionsController(IMediator mediator, ILogger<CollectionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CollectionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDetail(
            [FromRoute, Required] Guid id,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new GetCollectionByIdQuery(id), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving collection with ID: {CollectionId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while retrieving collection with ID {id}"
                });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(
            [FromBody, Required] string title,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new CreateCollectionCommand
                {
                    Title = title,
                    OwnerUserId = User.Id()
                }, ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating collection with title: {Title}", title);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An error occurred while creating collection"
                });
            }
        }

        [HttpPost("{id:guid}/books")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddBookToCollection(
            [FromRoute, Required] Guid id,
            [FromBody, Required] Guid bookId,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new AddBookToCollectionCommand(bookId, id), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book {BookId} to collection {CollectionId}", bookId, id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while adding book to collection"
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
            [FromBody, Required] UpdateCollectionDto updated,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new UpdateCollectionCommand(id, updated), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating collection with ID: {CollectionId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while updating collection with ID {id}"
                });
            }
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromRoute, Required] Guid id,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new DeleteCollectionCommand(id), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting collection with ID: {CollectionId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while deleting collection with ID {id}"
                });
            }
        }

        [HttpDelete("{collectionId:guid}/books/{bookId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveBookFromCollection(
            [FromRoute, Required] Guid collectionId,
            [FromRoute, Required] Guid bookId,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new RemoveBookFromCollectionCommand(bookId, collectionId), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error removing book {BookId} from collection {CollectionId}",
                    bookId, collectionId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while removing book from collection"
                });
            }
        }
    }
}