using LiveLib.Api.Common;
using LiveLib.Application.Features.Genres.CreateGenre;
using LiveLib.Application.Features.Genres.DeleteGenre;
using LiveLib.Application.Features.Genres.GetGenreById;
using LiveLib.Application.Features.Genres.GetGenres;
using LiveLib.Application.Features.Genres.UpdateGenre;
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
    public class GenresController : ControllerApiBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GenresController> _logger;

        public GenresController(IMediator mediator, ILogger<GenresController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GenreDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllGenres(CancellationToken ct)
        {
            try
            {
                var genres = await _mediator.Send(new GetGenresQuery(), ct);
                return Ok(genres);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all genres");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An error occurred while retrieving genres"
                });
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(GenreDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetGenreById(
            [FromRoute, Required] Guid id,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new GetGenreByIdQuery(id), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving genre with ID: {GenreId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while retrieving genre with ID {id}"
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateGenre(
            [FromBody, Required] CreateGenreCommand request,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(request, ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating genre");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An error occurred while creating genre"
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
        public async Task<IActionResult> UpdateGenre(
            [FromRoute, Required] Guid id,
            [FromBody, Required] UpdateGenreDto updatedGenre,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new UpdateGenreCommand(id, updatedGenre), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating genre with ID: {GenreId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while updating genre with ID {id}"
                });
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteGenre(
            [FromRoute, Required] Guid id,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new DeleteGenreCommand(id), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting genre with ID: {GenreId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while deleting genre with ID {id}"
                });
            }
        }
    }
}