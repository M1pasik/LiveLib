using LiveLib.Domain.Models;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace LiveLib.Application.Interfaces
{
    public interface IJwtProvider
    {
        string CookieName { get; }
        TimeSpan RefreshTokenExpiresDays { get; }
        TimeSpan AccessTokenExpiresMinutes { get; }
        string Issuer { get; }
        string Audience { get; }

        /// <summary>
        /// Generates new token pair for user
        /// </summary>
        Task<(string accessToken, string refreshToken)> GenerateTokensAsync(
            User user,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Extracts user principal from expired access token
        /// </summary>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        /// <summary>
        /// Gets user ID by valid refresh token
        /// </summary>
        Task<Guid> GetUserIdByRefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Refreshes token pair using valid refresh token
        /// </summary>
        Task<(string accessToken, string refreshToken)> RefreshTokensAsync(
            string expiredAccessToken,
            string refreshToken,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes all refresh tokens for user
        /// </summary>
        Task RevokeAllUserTokensAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes specific refresh token
        /// </summary>
        Task RevokeUserTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates refresh token
        /// </summary>
        Task<bool> ValidateRefreshToken(
            string refreshToken,
            CancellationToken cancellationToken = default);
    }
}