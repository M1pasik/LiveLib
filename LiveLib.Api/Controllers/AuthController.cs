using LiveLib.Api.Common;
using LiveLib.Api.Extensions;
using LiveLib.Api.Models;
using LiveLib.Application.Features.Users.CreateUser;
using LiveLib.Application.Features.Users.GetUserById;
using LiveLib.Application.Features.Users.GetUserByUsername;
using LiveLib.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace LiveLib.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    [Produces("application/json")]
    public class AuthController : ControllerApiBase
    {
        private readonly IMediator _mediator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        private CookieOptions CookieOptions => new()
        {
            Path = "/auth",
            Expires = DateTime.UtcNow.AddDays(15),
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = true,
            Domain = null 
        };

        public AuthController(
            IMediator mediator,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider,
            ITokenService tokenService,
            ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request, CancellationToken ct)
        {
            try
            {
                var refreshToken = Request.Cookies[_jwtProvider.CookieName];

                if (!string.IsNullOrEmpty(refreshToken) &&
                    await _jwtProvider.ValidateRefreshToken(refreshToken, ct))
                {
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Active session exists",
                        Detail = "You already have a valid session"
                    });
                }

                var userResult = await _mediator.Send(new GetUserByUsernameQuery(request.Username), ct);

                if (userResult.IsFailure)
                {
                    _logger.LogWarning("Login failed for username: {Username}", request.Username);
                    return Unauthorized(new ProblemDetails
                    {
                        Title = "Invalid credentials",
                        Detail = "Username or password is incorrect"
                    });
                }

                var user = userResult.Value;
                if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Invalid password for user: {UserId}", user.Id);
                    return Unauthorized(new ProblemDetails
                    {
                        Title = "Invalid credentials",
                        Detail = "Username or password is incorrect"
                    });
                }

                var tokens = await _jwtProvider.GenerateTokensAsync(user, ct);
                Response.Cookies.Append(_jwtProvider.CookieName, tokens.RefreshToken, CookieOptions);

                return Ok(new AuthResponse(tokens.AccessToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for username: {Username}", request.Username);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Login error",
                    Detail = "An error occurred while processing your request"
                });
            }
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            try
            {
                var refreshToken = Request.Cookies[_jwtProvider.CookieName];
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized(new ProblemDetails
                    {
                        Title = "No active session",
                        Detail = "No refresh token found"
                    });
                }

                await _jwtProvider.RevokeUserTokenAsync(refreshToken, ct);
                Response.Cookies.Delete(_jwtProvider.CookieName, CookieOptions);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Logout error",
                    Detail = "An error occurred while processing your request"
                });
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand request, CancellationToken ct)
        {
            try
            {
                var existingUser = await _mediator.Send(new GetUserByUsernameQuery(request.Name), ct);
                if (existingUser.IsSuccess)
                {
                    return Conflict(new ProblemDetails
                    {
                        Title = "User already exists",
                        Detail = $"Username '{request.Name}' is already taken"
                    });
                }

                var result = await _mediator.Send(request, ct);
                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for username: {Username}", request.Name);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Registration error",
                    Detail = "An error occurred while processing your request"
                });
            }
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshTokens(CancellationToken ct)
        {
            try
            {
                var refreshToken = Request.Cookies[_jwtProvider.CookieName];
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized(new ProblemDetails
                    {
                        Title = "Invalid refresh token",
                        Detail = "Refresh token is missing"
                    });
                }

                Response.Cookies.Delete(_jwtProvider.CookieName, CookieOptions);

                Guid userId;
                try
                {
                    userId = await _jwtProvider.GetUserIdByRefreshTokenAsync(refreshToken, ct);
                }
                catch (SecurityTokenException ex)
                {
                    return Unauthorized(new ProblemDetails
                    {
                        Title = "Invalid refresh token",
                        Detail = ex.Message
                    });
                }

                var userResult = await _mediator.Send(new GetUserByIdQuery(userId), ct);
                if (userResult.IsFailure)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "User not found",
                        Detail = $"User with ID {userId} not found"
                    });
                }

                await _jwtProvider.RevokeUserTokenAsync(refreshToken, ct);
                var tokens = await _jwtProvider.GenerateTokensAsync(userResult.Value, ct);

                Response.Cookies.Append(_jwtProvider.CookieName, tokens.RefreshToken, CookieOptions);

                return Ok(new AuthResponse(tokens.AccessToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Refresh error",
                    Detail = "An error occurred while processing your request"
                });
            }
        }

        [Authorize]
        [HttpPost("logoutAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LogoutFromAll(CancellationToken ct)
        {
            try
            {
                await _jwtProvider.RevokeAllUserTokensAsync(User.Id(), ct);
                Response.Cookies.Delete(_jwtProvider.CookieName, CookieOptions);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout from all devices for user: {UserId}", User.Id());
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Logout error",
                    Detail = "An error occurred while processing your request"
                });
            }
        }

        [Authorize]
        [HttpPost("revokeSession/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RevokeActiveSession(Guid sessionId, CancellationToken ct)
        {
            try
            {
                var token = await _tokenService.GetActiveTokenByIdAsync(sessionId, ct);
                if (token == null)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Invalid session",
                        Detail = $"Session with ID {sessionId} not found"
                    });
                }

                await _tokenService.RevokeTokenAsync(token, ct);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking session {SessionId} for user: {UserId}",
                    sessionId, User.Id());
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Revoke error",
                    Detail = "An error occurred while processing your request"
                });
            }
        }

        [Authorize]
        [HttpGet("activeSessions")]
        [ProducesResponseType(typeof(IEnumerable<ActiveSession>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveSessions(CancellationToken ct)
        {
            try
            {
                var sessions = await _tokenService.GetActiveTokensByUserIdAsync(User.Id(), ct);
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active sessions for user: {UserId}", User.Id());
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Server error",
                    Detail = "An error occurred while processing your request"
                });
            }
        }

        private record AuthResponse(string AccessToken);
    }
}