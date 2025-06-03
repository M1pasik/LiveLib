using LiveLib.Api.Common;
using LiveLib.Application.Features.Collections.GetCollectionsByUserId;
using LiveLib.Application.Features.Reviews.GetReviewsByUserId;
using LiveLib.Application.Features.Users.DeleteUser;
using LiveLib.Application.Features.Users.GetUserById;
using LiveLib.Application.Features.Users.GetUsers;
using LiveLib.Application.Features.Users.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LiveLib.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerApiBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("profile")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile(CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new GetUserByIdQuery(User.Id()), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile for user {UserId}", User.Id());
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An error occurred while retrieving user profile"
                });
            }
        }

        [HttpGet("profile/reviews")]
        [ProducesResponseType(typeof(IEnumerable<ReviewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserReviews(CancellationToken ct)
        {
            try
            {
                var reviews = await _mediator.Send(new GetReviewsByUserIdQuery(User.Id()), ct);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving reviews for user {UserId}", User.Id());
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An error occurred while retrieving user reviews"
                });
            }
        }

        [HttpGet("profile/collections")]
        [ProducesResponseType(typeof(IEnumerable<CollectionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserCollections(CancellationToken ct)
        {
            try
            {
                var collections = await _mediator.Send(new GetUserCollectionsByUserIdQuery(User.Id()), ct);
                return Ok(collections);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving collections for user {UserId}", User.Id());
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An error occurred while retrieving user collections"
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsers(CancellationToken ct)
        {
            try
            {
                var users = await _mediator.Send(new GetUsersQuery(), ct);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An error occurred while retrieving users"
                });
            }
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(
            [FromRoute, Required] Guid id,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new GetUserByIdQuery(id), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID: {UserId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while retrieving user with ID {id}"
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
                var result = await _mediator.Send(new DeleteUserCommand(id), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID: {UserId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while deleting user with ID {id}"
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
            [FromBody, Required] UserUpdateDto updatedUser,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new UpdateUserCommand(id, updatedUser), ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID: {UserId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = $"An error occurred while updating user with ID {id}"
                });
            }
        }
    }
}