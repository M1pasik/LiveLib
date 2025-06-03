using LiveLib.Api.Common;
using LiveLib.Application.Features.Books.CreateBook;
using LiveLib.Application.Features.Books.DeleteBook;
using LiveLib.Application.Features.Books.GetBookById;
using LiveLib.Application.Features.Books.GetBooks;
using LiveLib.Application.Features.Books.GetCover;
using LiveLib.Application.Features.Books.UpdateBook;
using LiveLib.Application.Features.Books.UploadCover;
using LiveLib.Application.Models.Books;
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
    public class BooksController : ControllerApiBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IMediator mediator, ILogger<BooksController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            try
            {
                var books = await _mediator.Send(new GetBooksQuery(), ct);
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all books");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An error occurred while retrieving books"
                });
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BookDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDetail(
            [FromRoute, Required] Guid id,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new GetBookByIdQuery(id), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book with ID: {BookId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while retrieving book with ID {id}"
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
            [FromBody, Required] CreateBookCommand request,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(request, ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An error occurred while creating book"
                });
            }
        }

        [HttpGet("{bookId:guid}/cover/{coverId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetBookCover(
            [FromRoute, Required] Guid bookId,
            [FromRoute, Required] string coverId,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new GetCoverQuery(bookId, coverId), ct);
                if (result.IsFailure)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "Not Found",
                        Detail = result.ErrorInfo?.Message ?? "Cover not found"
                    });
                }
                return File(result.Value!, "image/jpeg");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cover for book {BookId}", bookId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while retrieving cover for book {bookId}"
                });
            }
        }

        [HttpPost("{bookId:guid}/cover")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UploadBookImage(
            [FromRoute, Required] Guid bookId,
            [FromForm, Required] IFormFile image,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new UploadCoverCommand(bookId, image), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading cover for book {BookId}", bookId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while uploading cover for book {bookId}"
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
            [FromBody, Required] UpdateBookDto updated,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new UpdateBookCommand(id, updated), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book with ID: {BookId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while updating book with ID {id}"
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
                var result = await _mediator.Send(new DeleteBookCommand(id), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book with ID: {BookId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while deleting book with ID {id}"
                });
            }
        }
    }
}