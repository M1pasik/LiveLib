using System.Runtime.CompilerServices;
using System.Text.Json;
using LiveLib.Application.Interfaces;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly ICacheProvider _cache;
        private readonly JsonSerializerOptions _jsonOptions;

        public TokenService(ICacheProvider cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = false
            };
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken ct)
        {
            if (refreshToken == null) throw new ArgumentNullException(nameof(refreshToken));

            var expiration = refreshToken.ExpiresAt - DateTime.UtcNow;
            var tokenJson = JsonSerializer.Serialize(refreshToken, _jsonOptions);
            await NewMethod(refreshToken, expiration, tokenJson, ct);
        }

        private async Task NewMethod(RefreshToken refreshToken, TimeSpan expiration, string tokenJson, CancellationToken ct)
        {
            await Task.WhenAll(
                _cache.ObjectSetAsync($"token:{refreshToken.Id}", tokenJson, ct, expiration),
                _cache.StringSetAsync($"tokenId:{refreshToken.Token}", refreshToken.Id.ToString(), ct, expiration),
                _cache.SetAddAsync($"user:{refreshToken.UserId}:tokens", refreshToken.Token, ct, expiration)
            );
        }

        public async Task<RefreshToken?> GetActiveTokenAsync(string userRefreshToken, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(userRefreshToken))
                throw new ArgumentException("Refresh token cannot be null or whitespace", nameof(userRefreshToken));

            var tokenId = await _cache.StringGetAsync($"tokenId:{userRefreshToken}", ct);
            if (string.IsNullOrEmpty(tokenId)) return null;

            return await GetTokenFromCacheAsync(tokenId, ct);
        }

        public async Task<RefreshToken?> GetActiveTokenByIdAsync(Guid tokenId, CancellationToken ct)
        {
            if (tokenId == Guid.Empty)
                throw new ArgumentException("Token ID cannot be empty", nameof(tokenId));

            return await GetTokenFromCacheAsync(tokenId.ToString(), ct);
        }

        public async Task RevokeTokenAsync(RefreshToken refreshToken, CancellationToken ct)
        {
            if (refreshToken == null) throw new ArgumentNullException(nameof(refreshToken));

            await Task.WhenAll(
                _cache.RemoveAsync($"token:{refreshToken.Id}", ct),
                _cache.RemoveAsync($"tokenId:{refreshToken.Token}", ct),
                (Task<TResult>)_cache.SetRemoveAsync($"user:{refreshToken.UserId}:tokens", refreshToken.Token, ct)
            );
        }

        public async IAsyncEnumerable<RefreshToken> GetActiveTokensByUserIdAsync(
            Guid userId,
            [EnumeratorCancellation] CancellationToken ct)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty", nameof(userId));

            var tokens = await _cache.SetGetAsync($"user:{userId}:tokens", ct);

            foreach (var token in tokens)
            {
                ct.ThrowIfCancellationRequested();

                var refreshToken = await GetTokenByTokenStringAsync(token, ct);
                if (refreshToken != null)
                {
                    yield return refreshToken;
                }
            }
        }

        private async Task<RefreshToken?> GetTokenFromCacheAsync(string tokenKey, CancellationToken ct)
        {
            var tokenString = await _cache.StringGetAsync($"token:{tokenKey}", ct);
            return string.IsNullOrEmpty(tokenString)
                ? null
                : JsonSerializer.Deserialize<RefreshToken>(tokenString, _jsonOptions);
        }

        private async Task<RefreshToken?> GetTokenByTokenStringAsync(string token, CancellationToken ct)
        {
            var tokenId = await _cache.StringGetAsync($"tokenId:{token}", ct);
            if (string.IsNullOrEmpty(tokenId)) return null;

            return await GetTokenFromCacheAsync(tokenId, ct);
        }
    }
}