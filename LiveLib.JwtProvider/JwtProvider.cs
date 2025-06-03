using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LiveLib.Application.Interfaces;
using LiveLib.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LiveLib.JwtProvider
{
    public class JwtProvider : IJwtProvider
    {
        private readonly ITokenService _tokenService;
        public string CookieName { get; }
        public TimeSpan RefreshTokenExpiresDays { get; }
        public TimeSpan AccessTokenExpiresMinutes { get; }
        public string Issuer { get; }
        public string Audience { get; }
        private readonly string? _secretKey;

        public JwtProvider(IConfiguration configuration, ITokenService tokenService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));

            CookieName = configuration["JwtOptions:CookieName"] ?? "RefreshToken";
            RefreshTokenExpiresDays = TimeSpan.FromDays(
                int.Parse(configuration["JwtOptions:RefreshTokenExpiresDays"] ?? "15"));
            AccessTokenExpiresMinutes = TimeSpan.FromMinutes(
                int.Parse(configuration["JwtOptions:AccessTokenExpiresMinutes"] ?? "5"));
            Issuer = configuration["JwtOptions:Issuer"] ?? "DefaultIssuer";
            Audience = configuration["JwtOptions:Audience"] ?? "DefaultAudience";
            _secretKey = configuration["JwtOptions:SecretKey"];
        }

        public async Task<(string accessToken, string refreshToken)> GenerateTokensAsync(User user, CancellationToken cancellationToken = default)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            await SaveRefreshTokenAsync(user.Id, refreshToken, cancellationToken);
            return (accessToken, refreshToken);
        }

        public async Task<(string accessToken, string refreshToken)> RefreshTokensAsync(
            string expiredAccessToken,
            string refreshToken,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(expiredAccessToken))
                throw new ArgumentException("Access token cannot be null or empty", nameof(expiredAccessToken));
            if (string.IsNullOrEmpty(refreshToken))
                throw new ArgumentException("Refresh token cannot be null or empty", nameof(refreshToken));

            var principal = GetPrincipalFromExpiredToken(expiredAccessToken);

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier) ??
                throw new SecurityTokenException("Invalid access token: missing user identifier");

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
                throw new SecurityTokenException("Invalid user identifier in token");

            var storedRefreshToken = await _tokenService.GetActiveTokenAsync(refreshToken, cancellationToken) ??
                throw new SecurityTokenException("Invalid refresh token");

            if (!storedRefreshToken.IsActive || storedRefreshToken.ExpiresAt <= DateTime.UtcNow)
            {
                await _tokenService.RevokeTokenAsync(storedRefreshToken, cancellationToken);
                throw new SecurityTokenException("Refresh token has expired or been revoked");
            }

            await _tokenService.RevokeTokenAsync(storedRefreshToken, cancellationToken);

            var user = new User
            {
                Id = userId,
                Role = principal.FindFirst(ClaimTypes.Role)?.Value
            };

            return await GenerateTokensAsync(user, cancellationToken);
        }

        public async Task RevokeUserTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(token)) return;

            var refreshToken = await _tokenService.GetActiveTokenAsync(token, cancellationToken);
            if (refreshToken != null)
                await _tokenService.RevokeTokenAsync(refreshToken, cancellationToken);
        }

        public async Task RevokeAllUserTokensAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var userTokens = _tokenService.GetActiveTokensByUserIdAsync(userId, cancellationToken);
            await foreach (var token in userTokens)
            {
                await _tokenService.RevokeTokenAsync(token, cancellationToken);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            if (string.IsNullOrEmpty(_secretKey))
                throw new InvalidOperationException("Missing secret key for token signing");

            return ValidateJwtToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                ValidateLifetime = false
            });
        }

        public async Task<Guid> GetUserIdByRefreshTokenAsync(string token, CancellationToken ct)
        {
            var refreshToken = await _tokenService.GetActiveTokenAsync(token, ct) ??
                throw new SecurityTokenException("Invalid token");
            return refreshToken.UserId;
        }

        public async Task<bool> ValidateRefreshToken(string refreshToken, CancellationToken ct)
        {
            return await _tokenService.GetActiveTokenAsync(refreshToken, ct) != null;
        }

        private static ClaimsPrincipal ValidateJwtToken(string accessToken, TokenValidationParameters parameters)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, parameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private string GenerateAccessToken(User user)
        {
            if (string.IsNullOrEmpty(_secretKey))
                throw new InvalidOperationException("Missing secret key for token signing");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };

            if (!string.IsNullOrEmpty(user.Role))
                claims.Add(new Claim(ClaimTypes.Role, user.Role));

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(AccessTokenExpiresMinutes),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task SaveRefreshTokenAsync(Guid userId, string token, CancellationToken cancellationToken = default)
        {
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = token,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.Add(RefreshTokenExpiresDays),
                IsActive = true
            };

            await _tokenService.AddRefreshTokenAsync(refreshToken, cancellationToken);
        }
    }
}